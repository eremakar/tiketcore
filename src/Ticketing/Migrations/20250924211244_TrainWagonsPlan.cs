using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class TrainWagonsPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainWagonsPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainWagonsPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainWagonsPlans_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainWagonsPlanWagons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    PlanId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainWagonsPlanWagons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainWagonsPlanWagons_TrainWagonsPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "TrainWagonsPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainWagonsPlanWagons_Wagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "Wagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagonsPlans_TrainId",
                table: "TrainWagonsPlans",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagonsPlanWagons_PlanId",
                table: "TrainWagonsPlanWagons",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagonsPlanWagons_WagonId",
                table: "TrainWagonsPlanWagons",
                column: "WagonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainWagonsPlanWagons");

            migrationBuilder.DropTable(
                name: "TrainWagonsPlans");
        }
    }
}
