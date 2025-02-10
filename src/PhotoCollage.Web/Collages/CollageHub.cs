using Microsoft.AspNetCore.SignalR;
using PhotoCollage.Web.Client.Collages;
using Quartz;
using Quartz.Impl.Matchers;

namespace PhotoCollage.Web.Collages;

public interface ICollageClient
{
    Task ReceivePhoto(long photoId);
    Task ReceiveRemove(long photoId);
    Task ReceiveConnected(CollageSettings settings);
}

internal sealed class CollageHub : Hub<ICollageClient>
{
    private readonly CollageSettings settings = new();
    private readonly CollageHubConnectionManager connectionManager;
    private readonly ISchedulerFactory schedulerFactory;

    public CollageHub(
        CollageHubConnectionManager connectionManager,
        ISchedulerFactory schedulerFactory)
    {
        this.connectionManager = connectionManager;
        this.schedulerFactory = schedulerFactory;
    }

    public override async Task OnConnectedAsync()
    {
        await this.Clients.Caller.ReceiveConnected(this.settings);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var libraryIdList = this.connectionManager.GetLibraries(this.Context.ConnectionId);
        foreach (var libraryId in libraryIdList)
        {
            var libraryGroup = $"library-{libraryId}";
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, libraryGroup);
            this.connectionManager.RemoveClient(libraryId, this.Context.ConnectionId);
        }
    }

    public async Task StartCollage(CollageStartRequest request)
    {
        var libraryGroup = $"library-{request.LibraryId}";
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, libraryGroup);
        this.connectionManager.AddClient(request.LibraryId, this.Context.ConnectionId);

        await this.StartCollageJob(request.LibraryId);
    }
    private async Task StartCollageJob(int libraryId)
    {
        var scheduler = await this.schedulerFactory.GetScheduler();

        var jobData = new JobDataMap()
        {
            { nameof(CollageStartRequest.LibraryId), libraryId }
        };

        const string group = "collages";
        var name = $"collage-{libraryId}";

        var matcher = GroupMatcher<JobKey>.GroupEquals(group);
        var keys = await scheduler.GetJobKeys(matcher);
        if (keys.Any(x => x.Name == name))
        {
            return;
        }

        var job = JobBuilder.Create<CollageJob>()
            .WithIdentity(name, group)
            .UsingJobData(jobData)
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{name}-trigger", group)
            .ForJob(job)
            .StartNow()
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}
