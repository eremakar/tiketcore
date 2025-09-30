using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Tariff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TariffId",
                table: "SeatTariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IndexCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    VAT = table.Column<double>(type: "double precision", nullable: false),
                    BaseFareId = table.Column<long>(type: "bigint", nullable: true),
                    TrainCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tariffs_BaseFares_BaseFareId",
                        column: x => x.BaseFareId,
                        principalTable: "BaseFares",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tariffs_TrainCategories_TrainCategoryId",
                        column: x => x.TrainCategoryId,
                        principalTable: "TrainCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tariffs_Wagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "Wagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_TariffId",
                table: "SeatTariffs",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_BaseFareId",
                table: "Tariffs",
                column: "BaseFareId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_TrainCategoryId",
                table: "Tariffs",
                column: "TrainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_WagonId",
                table: "Tariffs",
                column: "WagonId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffs_Tariffs_TariffId",
                table: "SeatTariffs",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffs_Tariffs_TariffId",
                table: "SeatTariffs");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffs_TariffId",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "TariffId",
                table: "SeatTariffs");
        }
    }
}
