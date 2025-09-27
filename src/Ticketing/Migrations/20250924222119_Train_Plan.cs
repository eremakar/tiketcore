using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Train_Plan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlanId",
                table: "Trains",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_PlanId",
                table: "Trains",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainWagonsPlans_PlanId",
                table: "Trains",
                column: "PlanId",
                principalTable: "TrainWagonsPlans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainWagonsPlans_PlanId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_PlanId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Trains");
        }
    }
}
