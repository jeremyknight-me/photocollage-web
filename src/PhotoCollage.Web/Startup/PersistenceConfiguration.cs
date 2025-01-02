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
        Action<DbContextOptionsBuilder> contextOptionsBuilderAction =
            options => options.UseNpgsql(
                connectionString,
                sqlOptions =>
                {
                    var migrationAssembly = typeof(PhotoCollageContext).Assembly.GetName().Name;
                    _ = sqlOptions
                        .MigrationsAssembly(migrationAssembly)
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, PhotoCollageContext.Schema);
                });

        _ = builder.Services.AddDbContext<PhotoCollageContext>(
            contextOptionsBuilderAction,
            optionsLifetime: ServiceLifetime.Singleton);

        _ = builder.Services.AddDbContextFactory<PhotoCollageContext>(contextOptionsBuilderAction);

        return builder;
    }
}
