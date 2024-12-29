using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoCollage.Core.Entities;

namespace PhotoCollage.Persistence.Configuration;

internal sealed class ExcludedFolderConfiguration : IEntityTypeConfiguration<ExcludedFolder>
{
    public void Configure(EntityTypeBuilder<ExcludedFolder> builder)
    {
        builder.ToTable(nameof(ExcludedFolder));
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.RelativePath).IsRequired();

        builder
            .HasIndex([nameof(ExcludedFolder.Id), nameof(ExcludedFolder.RelativePath)])
            .IsUnique()
            .HasDatabaseName($"IX_{nameof(ExcludedFolder)}_RelativePath");

        builder.HasOne(p => p.Library)
            .WithMany(l => l.ExcludedFolders)
            .HasForeignKey(p => p.LibraryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
