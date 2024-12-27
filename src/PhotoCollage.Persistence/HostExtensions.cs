using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PhotoCollage.Persistence;

public static class HostExtensions
{
    public static IHost ApplyGarageMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PhotoCollageContext>();
        context.Database.Migrate();

        return host;
    }
}
