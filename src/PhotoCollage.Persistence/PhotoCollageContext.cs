using PhotoCollage.Core.Entities;
using PhotoCollage.Core.ValueObjects;
using PhotoCollage.Persistence.Configuration;
using PhotoCollage.Persistence.Converters;
using PhotoCollage.Persistence.Interceptors;

namespace PhotoCollage.Persistence;

public sealed class PhotoCollageContext : DbContext
{
    public const string Schema = "app";

    public PhotoCollageContext(DbContextOptions<PhotoCollageContext> options)
        : base(options)
    {
    }

    public DbSet<Library> Libraries { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().AreUnicode(unicode: true);

        configurationBuilder.Properties<ExcludedFolderId>().HaveConversion<ExcludedFolderIdValueConverter>();
        configurationBuilder.Properties<LibraryId>().HaveConversion<LibraryIdValueConverter>();
        configurationBuilder.Properties<PhotoId>().HaveConversion<PhotoIdValueConverter>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new EntityBaseSaveInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryConfiguration).Assembly);
    }
}
