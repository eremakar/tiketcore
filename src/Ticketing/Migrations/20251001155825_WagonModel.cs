using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class WagonModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TariffWagonItems_Wagons_WagonId",
                table: "TariffWagonItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainWagons_Wagons_WagonId",
                table: "TrainWagons");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainWagonsPlanWagons_Wagons_WagonId",
                table: "TrainWagonsPlanWagons");

            migrationBuilder.DropTable(
                name: "Wagons");

            migrationBuilder.CreateTable(
                name: "WagonModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SeatCount = table.Column<int>(type: "integer", nullable: false),
                    PictureS3 = table.Column<string>(type: "jsonb", nullable: true),
                    Class = table.Column<string>(type: "text", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagonModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WagonModels_WagonTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WagonTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WagonModels_TypeId",
                table: "WagonModels",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffWagonItems_WagonModels_WagonId",
                table: "TariffWagonItems",
                column: "WagonId",
                principalTable: "WagonModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainWagons_WagonModels_WagonId",
                table: "TrainWagons",
                column: "WagonId",
                principalTable: "WagonModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainWagonsPlanWagons_WagonModels_WagonId",
                table: "TrainWagonsPlanWagons",
                column: "WagonId",
                principalTable: "WagonModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TariffWagonItems_WagonModels_WagonId",
                table: "TariffWagonItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainWagons_WagonModels_WagonId",
                table: "TrainWagons");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainWagonsPlanWagons_WagonModels_WagonId",
                table: "TrainWagonsPlanWagons");

            migrationBuilder.DropTable(
                name: "WagonModels");

            migrationBuilder.CreateTable(
                name: "Wagons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    Class = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PictureS3 = table.Column<string>(type: "jsonb", nullable: true),
                    SeatCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wagons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wagons_WagonTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WagonTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wagons_TypeId",
                table: "Wagons",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TariffWagonItems_Wagons_WagonId",
                table: "TariffWagonItems",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainWagons_Wagons_WagonId",
                table: "TrainWagons",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainWagonsPlanWagons_Wagons_WagonId",
                table: "TrainWagonsPlanWagons",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id");
        }
    }
}
