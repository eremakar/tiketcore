using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Seat_WagonModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_TrainWagons_WagonId",
                table: "Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_WagonModels_WagonId",
                table: "Seats",
                column: "WagonId",
                principalTable: "WagonModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_WagonModels_WagonId",
                table: "Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_TrainWagons_WagonId",
                table: "Seats",
                column: "WagonId",
                principalTable: "TrainWagons",
                principalColumn: "Id");
        }
    }
}
