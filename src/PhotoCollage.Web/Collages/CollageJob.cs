using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PhotoCollage.Web.Client.Collages;
using Quartz;

namespace PhotoCollage.Web.Collages;

internal sealed class CollageJob : IJob
{
    private const int numberOfPhotos = 10;

    private readonly ConcurrentQueue<long> sentPhotosQueue = [];
    private readonly ConcurrentQueue<long> currentQueue = [];
    private readonly List<long> photoIds = [];

    private readonly ILogger<CollageJob> logger;
    private readonly IHubContext<CollageHub, ICollageClient> hubContext;
    private readonly ISender sender;
    private readonly CollageHubConnectionManager hubConnectionManager;

    public CollageJob(
        ILogger<CollageJob> jobLogger,
        IHubContext<CollageHub, ICollageClient> hubContext,
        ISender sender,
        CollageHubConnectionManager collageHubConnectionManager)
    {
        this.logger = jobLogger;
        this.hubContext = hubContext;
        this.sender = sender;
        this.hubConnectionManager = collageHubConnectionManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var jobData = context.MergedJobDataMap;
        var libraryId = jobData.GetInt(nameof(CollageStartRequest.LibraryId));
        if (libraryId == 0)
        {
            return;
        }

        if (this.logger.IsEnabled(LogLevel.Information))
        {
            this.logger.LogInformation("Collage worker ran at {time}", DateTimeOffset.Now);
        }

        while (this.hubConnectionManager.HasClients(libraryId))
        {
            try
            {
                await this.GetPhotos();
                await this.SendPhoto(libraryId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error sending photo.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }

    private async Task GetPhotos()
    {
        if (this.photoIds.Count > 0)
        {
            return;
        }

        var result = await this.sender.Send(new GetLibraryPhotosQuery());
        if (!result.IsSuccess)
        {
            return;
        }

        this.photoIds.Clear();
        this.photoIds.AddRange(result.Value);

        this.ResetQueue();
    }

    private async Task SendPhoto(int libraryId)
    {
        if (this.currentQueue.IsEmpty)
        {
            this.ResetQueue();
        }

        if (!this.currentQueue.TryDequeue(out var photoId))
        {
            return;
        }

        this.sentPhotosQueue.Enqueue(photoId);

        var group = $"library-{libraryId}";
        if (this.sentPhotosQueue.Count > numberOfPhotos
            && this.sentPhotosQueue.TryDequeue(out var result))
        {
            await this.hubContext.Clients.Group(group).ReceiveRemove(result);
        }

        await this.hubContext.Clients.Group(group).ReceivePhoto(photoId);
    }

    private void ResetQueue()
    {
        this.currentQueue.Clear();
        foreach (var id in this.photoIds.OrderBy(i => Random.Shared.Next()))
        {
            this.currentQueue.Enqueue(id);
        }
    }
}
