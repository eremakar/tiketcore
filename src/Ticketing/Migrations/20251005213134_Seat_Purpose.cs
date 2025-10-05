using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Seat_Purpose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PurposeId",
                table: "Seats",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeatPurposes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatPurposes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_PurposeId",
                table: "Seats",
                column: "PurposeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatPurposes_PurposeId",
                table: "Seats",
                column: "PurposeId",
                principalTable: "SeatPurposes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatPurposes_PurposeId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "SeatPurposes");

            migrationBuilder.DropIndex(
                name: "IX_Seats_PurposeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "PurposeId",
                table: "Seats");
        }
    }
}
