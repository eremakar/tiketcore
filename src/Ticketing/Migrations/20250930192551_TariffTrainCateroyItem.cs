using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class TariffTrainCateroyItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tariffs_TrainCategories_TrainCategoryId",
                table: "Tariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tariffs_Wagons_WagonId",
                table: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_Tariffs_TrainCategoryId",
                table: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_Tariffs_WagonId",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "IndexCoefficient",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "TrainCategoryId",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "WagonId",
                table: "Tariffs");

            migrationBuilder.CreateTable(
                name: "TariffTrainCategoryItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndexCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    TrainCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    TariffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffTrainCategoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffTrainCategoryItems_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TariffTrainCategoryItems_TrainCategories_TrainCategoryId",
                        column: x => x.TrainCategoryId,
                        principalTable: "TrainCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TariffWagonItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndexCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    TariffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffWagonItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffWagonItems_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TariffWagonItems_Wagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "Wagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TariffWagonTypeItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndexCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    WagonTypeId = table.Column<long>(type: "bigint", nullable: true),
                    TariffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffWagonTypeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffWagonTypeItems_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TariffWagonTypeItems_WagonTypes_WagonTypeId",
                        column: x => x.WagonTypeId,
                        principalTable: "WagonTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TariffSeatTypeItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndexCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: true),
                    TariffWagonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffSeatTypeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffSeatTypeItems_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TariffSeatTypeItems_TariffWagonItems_TariffWagonId",
                        column: x => x.TariffWagonId,
                        principalTable: "TariffWagonItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffSeatTypeItems_SeatTypeId",
                table: "TariffSeatTypeItems",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffSeatTypeItems_TariffWagonId",
                table: "TariffSeatTypeItems",
                column: "TariffWagonId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffTrainCategoryItems_TariffId",
                table: "TariffTrainCategoryItems",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffTrainCategoryItems_TrainCategoryId",
                table: "TariffTrainCategoryItems",
                column: "TrainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffWagonItems_TariffId",
                table: "TariffWagonItems",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffWagonItems_WagonId",
                table: "TariffWagonItems",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffWagonTypeItems_TariffId",
                table: "TariffWagonTypeItems",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffWagonTypeItems_WagonTypeId",
                table: "TariffWagonTypeItems",
                column: "WagonTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffSeatTypeItems");

            migrationBuilder.DropTable(
                name: "TariffTrainCategoryItems");

            migrationBuilder.DropTable(
                name: "TariffWagonTypeItems");

            migrationBuilder.DropTable(
                name: "TariffWagonItems");

            migrationBuilder.AddColumn<double>(
                name: "IndexCoefficient",
                table: "Tariffs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "TrainCategoryId",
                table: "Tariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WagonId",
                table: "Tariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_TrainCategoryId",
                table: "Tariffs",
                column: "TrainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_WagonId",
                table: "Tariffs",
                column: "WagonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tariffs_TrainCategories_TrainCategoryId",
                table: "Tariffs",
                column: "TrainCategoryId",
                principalTable: "TrainCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tariffs_Wagons_WagonId",
                table: "Tariffs",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id");
        }
    }
}
