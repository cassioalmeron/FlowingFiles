using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowingFiles.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var isSqlite = migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite";
            var boolType = isSqlite ? "INTEGER" : "boolean";

            migrationBuilder.CreateTable(
                name: "EmailDestination",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailAddress = table.Column<string>(maxLength: 320, nullable: false),
                    Active = table.Column<bool>(type: boolType, nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailDestination", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailDestination_EmailAddress",
                table: "EmailDestination",
                column: "EmailAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailDestination");
        }
    }
}
