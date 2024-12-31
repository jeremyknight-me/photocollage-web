namespace PhotoCollage.Web.Startup;

internal static class SettingsConfiguration
{
    internal static WebApplicationBuilder SetupApplicationSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<ApplicationSettings>()
            .BindConfiguration(ApplicationSettings.Path)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return builder;
    }
}
