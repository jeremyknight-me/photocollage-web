using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace PhotoCollage.Web.Collages;

internal sealed class CollageWorker : BackgroundService
{
    private const int numberOfPhotos = 10;

    private readonly ConcurrentQueue<long> photoIdQueue = [];

    private readonly ILogger<CollageWorker> logger;
    private readonly IHubContext<CollageHub, ICollageClient> hubContext;
    private readonly CollageHubConnectionManager hubConnectionManager;

    public CollageWorker(
        ILogger<CollageWorker> workerLogger,
        IHubContext<CollageHub, ICollageClient> hubContext,
        CollageHubConnectionManager collageHubConnectionManager)
    {
        this.logger = workerLogger;
        this.hubContext = hubContext;
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

    private async Task SendPhoto()
    {
        if (!this.hubConnectionManager.HasClients)
        {
            if (!this.photoIdQueue.IsEmpty)
            {
                this.photoIdQueue.Clear();
            }

            return;
        }

        // todo: get only photos with status set to sync
        long photoId;
        do
        {
            photoId = Random.Shared.Next(int.MaxValue);
        } while (this.photoIdQueue.Contains(photoId));

        this.photoIdQueue.Enqueue(photoId);

        if (this.photoIdQueue.Count > numberOfPhotos
            && this.photoIdQueue.TryDequeue(out var result))
        {
            await this.hubContext.Clients.Group(CollageHub.ConnectedGroupName).ReceiveRemove(result);
        }

        await this.hubContext.Clients.Group(CollageHub.ConnectedGroupName).ReceivePhoto(photoId);
    }
}
