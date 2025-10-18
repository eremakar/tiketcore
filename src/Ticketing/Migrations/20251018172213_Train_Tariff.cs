using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Train_Tariff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TariffId",
                table: "Trains",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TariffId",
                table: "Trains",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Tariffs_TariffId",
                table: "Trains",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Tariffs_TariffId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TariffId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TariffId",
                table: "Trains");
        }
    }
}
