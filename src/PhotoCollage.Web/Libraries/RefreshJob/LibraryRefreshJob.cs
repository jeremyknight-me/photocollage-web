using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhotoCollage.Core.Extensions;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence;
using Quartz;

namespace PhotoCollage.Web.Libraries.RefreshJob;

internal sealed class LibraryRefreshJob : IJob
{
    public static readonly JobKey Key = new(Name, "LibraryJobsRepeating");
    public const string Name = nameof(LibraryRefreshJob);

    private readonly ILogger<LibraryRefreshJob> logger;
    private readonly PhotoCollageContext context;
    private readonly string environment;
    private readonly ApplicationSettings settings;

    public LibraryRefreshJob(
        ILogger<LibraryRefreshJob> jobLogger,
        IHostEnvironment hostEnvironment,
        IOptionsSnapshot<ApplicationSettings> optionsSnapshot,
        PhotoCollageContext photoCollageContext)
    {
        this.logger = jobLogger;
        this.context = photoCollageContext;
        this.environment = hostEnvironment.EnvironmentName;
        this.settings = optionsSnapshot.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            if (!this.settings.LibraryRefreshJob.Enabled)
            {
                return;
            }

            this.logger.LogQuartzJobStarted(Name, this.GetTime(), this.environment);
            await this.Refresh();
        }
        catch (AggregateException ex)
        {
            foreach (var exception in ex.InnerExceptions)
            {
                this.logger.LogQuartzJobErrored(Name, ex);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogQuartzJobErrored(Name, ex);
        }
    }

    private DateTimeOffset GetTime() => DateTimeOffset.Now;

    private async Task Refresh()
    {
        var libraries = await this.context.Libraries
            .Include(l => l.ExcludedFolders)
            .Include(l => l.Photos)
            .AsSplitQuery()
            .ToArrayAsync();
        if (libraries.Length == 0)
        {
            return;
        }

        var files = this.GetPhotoFiles();
        foreach (var library in libraries)
        {
            _ = library.Refresh(files);
            _ = await this.context.SaveChangesAsync();
        }
    }

    private PhotoFile[] GetPhotoFiles()
    {
        DirectoryInfo directory = new(this.settings.PhotoRootDirectory);
        return directory
            .EnumerateFiles("*.*", SearchOption.AllDirectories)
            .GetPhotoFiles(this.settings.PhotoRootDirectory.Length);
    }
}
