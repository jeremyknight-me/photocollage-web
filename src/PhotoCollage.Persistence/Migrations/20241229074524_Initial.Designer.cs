﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PhotoCollage.Persistence;

#nullable disable

namespace PhotoCollage.Persistence.Migrations
{
    [DbContext(typeof(PhotoCollageContext))]
    [Migration("20241229074524_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PhotoCollage.Core.Entities.ExcludedFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LibraryId")
                        .HasColumnType("integer");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.HasIndex("Id", "RelativePath")
                        .IsUnique()
                        .HasDatabaseName("IX_ExcludedFolder_RelativePath");

                    b.ToTable("ExcludedFolder", "app");
                });

            modelBuilder.Entity("PhotoCollage.Core.Entities.Library", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Library", "app");
                });

            modelBuilder.Entity("PhotoCollage.Core.Entities.Photo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LibraryId")
                        .HasColumnType("integer");

                    b.Property<int>("ProcessAction")
                        .HasColumnType("integer");

                    b.Property<string>("RelativePath")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("text");

                    b.Property<long>("SizeBytes")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.HasIndex("Id", "RelativePath")
                        .IsUnique()
                        .HasDatabaseName("IX_Photo_RelativePath");

                    b.ToTable("Photo", "app");
                });

            modelBuilder.Entity("PhotoCollage.Core.Entities.ExcludedFolder", b =>
                {
                    b.HasOne("PhotoCollage.Core.Entities.Library", "Library")
                        .WithMany("ExcludedFolders")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("PhotoCollage.Core.Entities.Photo", b =>
                {
                    b.HasOne("PhotoCollage.Core.Entities.Library", "Library")
                        .WithMany("Photos")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("PhotoCollage.Core.Entities.Library", b =>
                {
                    b.Navigation("ExcludedFolders");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
