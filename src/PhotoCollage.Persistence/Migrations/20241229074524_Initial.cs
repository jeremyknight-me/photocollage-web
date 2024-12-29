using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PhotoCollage.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "app");

        migrationBuilder.CreateTable(
            name: "Library",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Library", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ExcludedFolder",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                LibraryId = table.Column<int>(type: "integer", nullable: false),
                RelativePath = table.Column<string>(type: "text", nullable: false),
                DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExcludedFolder", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExcludedFolder_Library_LibraryId",
                    column: x => x.LibraryId,
                    principalSchema: "app",
                    principalTable: "Library",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Photo",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                LibraryId = table.Column<int>(type: "integer", nullable: false),
                RelativePath = table.Column<string>(type: "text", nullable: false),
                ProcessAction = table.Column<int>(type: "integer", nullable: false),
                SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Photo", x => x.Id);
                table.ForeignKey(
                    name: "FK_Photo_Library_LibraryId",
                    column: x => x.LibraryId,
                    principalSchema: "app",
                    principalTable: "Library",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_ExcludedFolder_LibraryId",
            schema: "app",
            table: "ExcludedFolder",
            column: "LibraryId");

        migrationBuilder.CreateIndex(
            name: "IX_ExcludedFolder_RelativePath",
            schema: "app",
            table: "ExcludedFolder",
            columns: new[] { "Id", "RelativePath" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Photo_LibraryId",
            schema: "app",
            table: "Photo",
            column: "LibraryId");

        migrationBuilder.CreateIndex(
            name: "IX_Photo_RelativePath",
            schema: "app",
            table: "Photo",
            columns: new[] { "Id", "RelativePath" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ExcludedFolder",
            schema: "app");

        migrationBuilder.DropTable(
            name: "Photo",
            schema: "app");

        migrationBuilder.DropTable(
            name: "Library",
            schema: "app");
    }
}
