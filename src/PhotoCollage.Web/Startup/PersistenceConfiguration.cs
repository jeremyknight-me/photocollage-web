using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PhotoCollage.Persistence;

namespace PhotoCollage.Web.Startup;

internal static class PersistenceConfiguration
{
    internal static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PhotoCollageContext>();
        context.Database.Migrate();

        return app;
    }

    internal static WebApplicationBuilder SetupPersistence(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PhotoCollage");
        _ = builder.Services.AddDbContext<PhotoCollageContext>(options => options
            .UseNpgsql(connectionString, sqlOptions =>
            {
                var migrationAssembly = typeof(PhotoCollageContext).Assembly.GetName().Name;
                _ = sqlOptions
                    .MigrationsAssembly(migrationAssembly)
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, PhotoCollageContext.Schema);
            }));

        return builder;
    }
}
