using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Carrier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "WagonModels");

            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "WagonModels",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carriers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    BIN = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WagonModels_ClassId",
                table: "WagonModels",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_WagonModels_WagonClasses_ClassId",
                table: "WagonModels",
                column: "ClassId",
                principalTable: "WagonClasses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WagonModels_WagonClasses_ClassId",
                table: "WagonModels");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropIndex(
                name: "IX_WagonModels_ClassId",
                table: "WagonModels");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "WagonModels");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "WagonModels",
                type: "text",
                nullable: true);
        }
    }
}
