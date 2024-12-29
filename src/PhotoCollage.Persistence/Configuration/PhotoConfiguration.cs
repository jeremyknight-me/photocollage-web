﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoCollage.Core.Entities;

namespace PhotoCollage.Persistence.Configuration;

internal sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable(nameof(Photo));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.RelativePath).IsRequired();
        builder.Property(p => p.ProcessAction).IsRequired();
        builder.Property(p => p.SizeBytes).IsRequired();

        builder
            .HasIndex([nameof(Photo.Id), nameof(Photo.RelativePath)])
            .IsUnique()
            .HasDatabaseName($"IX_{nameof(Photo)}_RelativePath");

        builder.HasOne(p => p.Library)
            .WithMany(l => l.Photos)
            .HasForeignKey(p => p.LibraryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}