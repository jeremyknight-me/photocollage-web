using PhotoCollage.Web.Libraries;

namespace PhotoCollage.Web.Startup;

internal static class EndpointConfiguration
{
    internal static WebApplication UseCustomEndpoints(this WebApplication app)
    {
        app.MapLibraryEndpoints();
        return app;
    }
}
