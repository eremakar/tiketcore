using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Wagon_WagonType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Wagons");

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Wagons",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WagonTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagonTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wagons_TypeId",
                table: "Wagons",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wagons_WagonTypes_TypeId",
                table: "Wagons",
                column: "TypeId",
                principalTable: "WagonTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wagons_WagonTypes_TypeId",
                table: "Wagons");

            migrationBuilder.DropTable(
                name: "WagonTypes");

            migrationBuilder.DropIndex(
                name: "IX_Wagons_TypeId",
                table: "Wagons");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Wagons");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Wagons",
                type: "text",
                nullable: true);
        }
    }
}
