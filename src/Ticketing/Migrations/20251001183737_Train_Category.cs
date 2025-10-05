using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Train_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Trains",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_CategoryId",
                table: "Trains",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainCategories_CategoryId",
                table: "Trains",
                column: "CategoryId",
                principalTable: "TrainCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainCategories_CategoryId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_CategoryId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Trains");
        }
    }
}
