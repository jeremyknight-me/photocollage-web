using Microsoft.EntityFrameworkCore.Design;
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
        DbContextOptionsBuilder<PhotoCollageContext> builder = new();
        _ = builder.UseNpgsql(
            connectionString: this.configuration.GetConnectionString("DesignTime"),
            options => options.MigrationsAssembly(ServiceCollectionExtensions.MigrationAssembly));
        return new PhotoCollageContext(builder.Options);
    }
}
