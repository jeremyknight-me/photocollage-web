namespace PhotoCollage.Web.Libraries;

internal static partial class QuartzLoggerExtensions
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Job {JobName} running at {Time} in {Environment}")]
    public static partial void LogQuartzJobStarted(
        this ILogger logger,
        string jobName,
        DateTimeOffset time,
        string environment);

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Job {JobName} failed")]
    public static partial void LogQuartzJobErrored(
        this ILogger logger,
        string jobName,
        Exception ex);
}
