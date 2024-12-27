using PhotoCollage.Persistence.Interceptors;

namespace PhotoCollage.Persistence;

public sealed class PhotoCollageContext : DbContext
{
    public const string Schema = "app";

    public PhotoCollageContext(DbContextOptions<PhotoCollageContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().AreUnicode(unicode: true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new EntityBaseSaveInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }
}
