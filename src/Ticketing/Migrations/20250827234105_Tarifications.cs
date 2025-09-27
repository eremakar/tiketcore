using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Tarifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Wagons_WagonId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Seats");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Tickets",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SeatSegments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Seats",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SeatReservations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SeatCountSegments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "SeatCountReservations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "BaseFares",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseFares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    TarifCoefficient = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    TarifCoefficient = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    TarifCoefficient = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WagonClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    TarifCoefficient = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagonClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatTariffHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseFareId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    TrainCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    WagonClassId = table.Column<long>(type: "bigint", nullable: true),
                    SeasonId = table.Column<long>(type: "bigint", nullable: true),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ConnectionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTariffHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_BaseFares_BaseFareId",
                        column: x => x.BaseFareId,
                        principalTable: "BaseFares",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_TrainCategories_TrainCategoryId",
                        column: x => x.TrainCategoryId,
                        principalTable: "TrainCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffHistories_WagonClasses_WagonClassId",
                        column: x => x.WagonClassId,
                        principalTable: "WagonClasses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeatTariffs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    BaseFareId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    TrainCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    WagonClassId = table.Column<long>(type: "bigint", nullable: true),
                    SeasonId = table.Column<long>(type: "bigint", nullable: true),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ConnectionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatTariffs_BaseFares_BaseFareId",
                        column: x => x.BaseFareId,
                        principalTable: "BaseFares",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_TrainCategories_TrainCategoryId",
                        column: x => x.TrainCategoryId,
                        principalTable: "TrainCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatTariffs_WagonClasses_WagonClassId",
                        column: x => x.WagonClassId,
                        principalTable: "WagonClasses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TypeId",
                table: "Seats",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_BaseFareId",
                table: "SeatTariffHistories",
                column: "BaseFareId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_ConnectionId",
                table: "SeatTariffHistories",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_SeasonId",
                table: "SeatTariffHistories",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_SeatTypeId",
                table: "SeatTariffHistories",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_TrainCategoryId",
                table: "SeatTariffHistories",
                column: "TrainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_TrainId",
                table: "SeatTariffHistories",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffHistories_WagonClassId",
                table: "SeatTariffHistories",
                column: "WagonClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_BaseFareId",
                table: "SeatTariffs",
                column: "BaseFareId");

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
                name: "IX_SeatTariffs_TrainCategoryId",
                table: "SeatTariffs",
                column: "TrainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_TrainId",
                table: "SeatTariffs",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatTariffs_WagonClassId",
                table: "SeatTariffs",
                column: "WagonClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatTypes_TypeId",
                table: "Seats",
                column: "TypeId",
                principalTable: "SeatTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_TrainWagons_WagonId",
                table: "Seats",
                column: "WagonId",
                principalTable: "TrainWagons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatTypes_TypeId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_TrainWagons_WagonId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "SeatTariffHistories");

            migrationBuilder.DropTable(
                name: "SeatTariffs");

            migrationBuilder.DropTable(
                name: "BaseFares");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "SeatTypes");

            migrationBuilder.DropTable(
                name: "TrainCategories");

            migrationBuilder.DropTable(
                name: "WagonClasses");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TypeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatSegments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatReservations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatCountSegments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatCountReservations");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Wagons_WagonId",
                table: "Seats",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id");
        }
    }
}
