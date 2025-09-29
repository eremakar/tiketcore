using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class SeatTariffItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffs_Connections_ConnectionId",
                table: "SeatTariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffs_Seasons_SeasonId",
                table: "SeatTariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffs_SeatTypes_SeatTypeId",
                table: "SeatTariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatTariffs_WagonClasses_WagonClassId",
                table: "SeatTariffs");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffs_ConnectionId",
                table: "SeatTariffs");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffs_SeasonId",
                table: "SeatTariffs");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffs_SeatTypeId",
                table: "SeatTariffs");

            migrationBuilder.DropIndex(
                name: "IX_SeatTariffs_WagonClassId",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "SeatTypeId",
                table: "SeatTariffs");

            migrationBuilder.DropColumn(
                name: "WagonClassId",
                table: "SeatTariffs");

            migrationBuilder.CreateTable(
                name: "SeatTariffItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Distance = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    WagonClassId = table.Column<long>(type: "bigint", nullable: true),
                    SeasonId = table.Column<long>(type: "bigint", nullable: true),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: true),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    SeatTariffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTariffItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_SeatTariffs_SeatTariffId",
                        column: x => x.SeatTariffId,
                        principalTable: "SeatTariffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffItems_WagonClasses_WagonClassId",
                        column: x => x.WagonClassId,
                        principalTable: "WagonClasses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_FromId",
                table: "SeatTariffItems",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_SeasonId",
                table: "SeatTariffItems",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_SeatTariffId",
                table: "SeatTariffItems",
                column: "SeatTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_SeatTypeId",
                table: "SeatTariffItems",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_ToId",
                table: "SeatTariffItems",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffItems_WagonClassId",
                table: "SeatTariffItems",
                column: "WagonClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeatTariffItems");

            migrationBuilder.AddColumn<long>(
                name: "ConnectionId",
                table: "SeatTariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SeatTariffs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "SeasonId",
                table: "SeatTariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SeatTypeId",
                table: "SeatTariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WagonClassId",
                table: "SeatTariffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_ConnectionId",
                table: "SeatTariffs",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_SeasonId",
                table: "SeatTariffs",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_SeatTypeId",
                table: "SeatTariffs",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_WagonClassId",
                table: "SeatTariffs",
                column: "WagonClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffs_Connections_ConnectionId",
                table: "SeatTariffs",
                column: "ConnectionId",
                principalTable: "Connections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffs_Seasons_SeasonId",
                table: "SeatTariffs",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffs_SeatTypes_SeatTypeId",
                table: "SeatTariffs",
                column: "SeatTypeId",
                principalTable: "SeatTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTariffs_WagonClasses_WagonClassId",
                table: "SeatTariffs",
                column: "WagonClassId",
                principalTable: "WagonClasses",
                principalColumn: "Id");
        }
    }
}
