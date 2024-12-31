using PhotoCollage.Web.Libraries.RefreshJob;
using Quartz;

namespace PhotoCollage.Web.Startup;

internal static class QuartzConfiguration
{
    internal static WebApplicationBuilder SetupQuartz(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddQuartz(q =>
        {
            q.AddPhotoLibraryRefreshJob();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return builder;
    }

    private static IServiceCollectionQuartzConfigurator AddPhotoLibraryRefreshJob(this IServiceCollectionQuartzConfigurator quartz)
    {
        quartz.AddJob<LibraryRefreshJob>(j => j
            .WithIdentity(LibraryRefreshJob.Key)
            .DisallowConcurrentExecution());

        quartz.AddTrigger(t => t
            .WithIdentity($"{LibraryRefreshJob.Name}-{Guid.NewGuid():N}")
            .ForJob(LibraryRefreshJob.Key)
            .StartNow()
            .WithSimpleSchedule(s => s
                .WithIntervalInHours(2)
                .RepeatForever()
            )
        );

        return quartz;
    }
}
