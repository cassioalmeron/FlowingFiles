using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace FlowingFiles.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentSample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";

            // pgvector has no SQLite equivalent (Design Decision 3, plan 003): the extension and the
            // vector(1024) column type only apply when targeting Postgres.
            if (!isSqlite)
                migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS vector;");

            migrationBuilder.CreateTable(
                name: "DocumentSample",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentOptionId = table.Column<int>(nullable: false),
                    SourceFileName = table.Column<string>(maxLength: 256, nullable: false),
                    ExtractedText = table.Column<string>(nullable: false),
                    Embedding = isSqlite
                        ? table.Column<byte[]>(type: "BLOB", nullable: false)
                        : table.Column<Vector>(type: "vector(1024)", nullable: false),
                    // Explicit type required here: without it, this column came out as Postgres `text`
                    // instead of `timestamp without time zone` when this migration ran against Npgsql
                    // (see Flowing-EFCore skill's DateTime troubleshooting note) — reads then threw
                    // InvalidCastException. SQLite has always been fine either way.
                    CreatedAt = table.Column<DateTime>(type: isSqlite ? "TEXT" : "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentSample_DocumentOption_DocumentOptionId",
                        column: x => x.DocumentOptionId,
                        principalTable: "DocumentOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSample_DocumentOptionId",
                table: "DocumentSample",
                column: "DocumentOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSample");
        }
    }
}
