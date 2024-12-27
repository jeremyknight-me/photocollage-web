using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace PhotoCollage.Persistence;

public static class ServiceCollectionExtensions
{
    internal const string MigrationAssembly = "PhotoCollage.Persistence";

    public static IServiceCollection AddPhotoCollagePersistence(this IServiceCollection services, string connectionString)
    {
        _ = services.AddDbContext<PhotoCollageContext>(options => options
            .UseNpgsql(connectionString, sqlOptions =>
            {
                _ = sqlOptions
                    .MigrationsAssembly(MigrationAssembly)
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, PhotoCollageContext.Schema);
            }));

        return services;
    }
}
