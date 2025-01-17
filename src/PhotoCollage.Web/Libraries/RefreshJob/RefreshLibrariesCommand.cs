using Ardalis.Result;
using PhotoCollage.Web.Helpers.Commands;
using Quartz;

namespace PhotoCollage.Web.Libraries.RefreshJob;

internal sealed class RefreshLibrariesCommand : ICommand
{
}

internal sealed class RefreshLibrariesCommandHandler : ICommandHandler<RefreshLibrariesCommand>
{
    private readonly ISchedulerFactory schedulerFactory;

    public RefreshLibrariesCommandHandler(ISchedulerFactory quartzSchedulerFactory)
    {
        this.schedulerFactory = quartzSchedulerFactory;
    }

    public async Task<Result> Handle(RefreshLibrariesCommand request, CancellationToken cancellationToken)
    {
        var scheduler = await this.schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.TriggerJob(LibraryRefreshJob.Key, cancellationToken);
        return Result.Success();
    }
}
