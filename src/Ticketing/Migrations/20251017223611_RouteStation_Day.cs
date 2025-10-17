using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class RouteStation_Day : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "RouteStations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "RouteStations");
        }
    }
}
