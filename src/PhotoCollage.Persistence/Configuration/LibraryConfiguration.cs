using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoCollage.Core.Entities;

namespace PhotoCollage.Persistence.Configuration;

internal sealed class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.ToTable(nameof(Library));
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
    }
}
