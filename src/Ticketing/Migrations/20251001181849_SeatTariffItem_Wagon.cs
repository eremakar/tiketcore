using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class SeatTariffItem_Wagon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WagonId",
                table: "SeatTariffItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_WagonId",
                table: "SeatTariffItems",
                column: "WagonId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffItems_WagonModels_WagonId",
                table: "SeatTariffItems",
                column: "WagonId",
                principalTable: "WagonModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffItems_WagonModels_WagonId",
                table: "SeatTariffItems");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffItems_WagonId",
                table: "SeatTariffItems");

            migrationBuilder.DropColumn(
                name: "WagonId",
                table: "SeatTariffItems");
        }
    }
}
