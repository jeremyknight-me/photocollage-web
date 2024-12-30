using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace PhotoCollage.Persistence;

public sealed class PhotoCollageContextDesignTimeFactory : IDesignTimeDbContextFactory<PhotoCollageContext>
{
    private readonly IConfiguration configuration;

    public PhotoCollageContextDesignTimeFactory()
    {
        ConfigurationBuilder builder = new();
        _ = builder.AddUserSecrets(this.GetType().Assembly);
        this.configuration = builder.Build();
    }

    public PhotoCollageContext CreateDbContext(string[] args)
    {
        var migrationAssembly = typeof(PhotoCollageContext).Assembly.GetName().Name;
        DbContextOptionsBuilder<PhotoCollageContext> builder = new();
        _ = builder.UseNpgsql(
            connectionString: this.configuration.GetConnectionString("DesignTime"),
            options => options
                .MigrationsAssembly(migrationAssembly)
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, PhotoCollageContext.Schema));
        return new PhotoCollageContext(builder.Options);
    }
}
