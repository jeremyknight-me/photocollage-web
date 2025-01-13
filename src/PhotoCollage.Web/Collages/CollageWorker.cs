using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace PhotoCollage.Web.Collages;

internal sealed class CollageWorker : BackgroundService
{
    private const int numberOfPhotos = 10;

    private readonly ConcurrentQueue<long> sentPhotosQueue = [];
    private readonly ConcurrentQueue<long> currentQueue = [];
    private readonly List<long> photoIds = [];

    private readonly ILogger<CollageWorker> logger;
    private readonly IHubContext<CollageHub, ICollageClient> hubContext;
    private readonly ISender sender;
    private readonly CollageHubConnectionManager hubConnectionManager;

    public CollageWorker(
        ILogger<CollageWorker> workerLogger,
        IHubContext<CollageHub, ICollageClient> hubContext,
        ISender sender,
        CollageHubConnectionManager collageHubConnectionManager)
    {
        this.logger = workerLogger;
        this.hubContext = hubContext;
        this.sender = sender;
        this.hubConnectionManager = collageHubConnectionManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (this.logger.IsEnabled(LogLevel.Information))
                {
                    this.logger.LogInformation("Collage worker ran at {time}", DateTimeOffset.Now);
                }

                if (!this.hubConnectionManager.HasClients)
                {
                    if (!this.sentPhotosQueue.IsEmpty)
                    {
                        this.sentPhotosQueue.Clear();
                    }

                    continue;
                }

                await this.GetPhotos();
                await this.SendPhoto();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error sending photo.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
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

    private async Task SendPhoto()
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

        if (this.sentPhotosQueue.Count > numberOfPhotos
            && this.sentPhotosQueue.TryDequeue(out var result))
        {
            await this.hubContext.Clients.Group(CollageHub.ConnectedGroupName).ReceiveRemove(result);
        }

        await this.hubContext.Clients.Group(CollageHub.ConnectedGroupName).ReceivePhoto(photoId);
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
