using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PhotoCollage.Web.Startup;

internal static class OpenTelemetryConfiguration
{
    internal static WebApplicationBuilder SetupOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("PhotoCollage"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddQuartzInstrumentation();
                tracing.AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());
        return builder;
    }
}
