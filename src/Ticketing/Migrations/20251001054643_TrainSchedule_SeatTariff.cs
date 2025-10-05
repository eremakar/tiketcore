using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class TrainSchedule_SeatTariff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SeatTariffId",
                table: "TrainSchedules",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainSchedules_SeatTariffId",
                table: "TrainSchedules",
                column: "SeatTariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainSchedules_SeatTariffs_SeatTariffId",
                table: "TrainSchedules",
                column: "SeatTariffId",
                principalTable: "SeatTariffs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainSchedules_SeatTariffs_SeatTariffId",
                table: "TrainSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TrainSchedules_SeatTariffId",
                table: "TrainSchedules");

            migrationBuilder.DropColumn(
                name: "SeatTariffId",
                table: "TrainSchedules");
        }
    }
}
