﻿using PhotoCollage.Web.Client.Collages;
using PhotoCollage.Web.Collages;

namespace PhotoCollage.Web.Startup;

internal static class CollageConfiguration
{
    internal static WebApplication UseCollage(this WebApplication app)
    {
        app.MapHub<CollageHub>(CollageLiterals.HubRelativeUrl);
        app.MapCollageEndpoints();
        return app;
    }

    internal static WebApplicationBuilder SetupCollage(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<CollageHubConnectionManager>();
        return builder;
    }
}
