using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class WagonFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CarrierId",
                table: "TrainWagons",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WagonFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagonFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WagonModelFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    FeatureId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagonModelFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WagonModelFeatures_WagonFeatures_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "WagonFeatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WagonModelFeatures_WagonModels_WagonId",
                        column: x => x.WagonId,
                        principalTable: "WagonModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagons_CarrierId",
                table: "TrainWagons",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_WagonModelFeatures_FeatureId",
                table: "WagonModelFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WagonModelFeatures_WagonId",
                table: "WagonModelFeatures",
                column: "WagonId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainWagons_Carriers_CarrierId",
                table: "TrainWagons",
                column: "CarrierId",
                principalTable: "Carriers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainWagons_Carriers_CarrierId",
                table: "TrainWagons");

            migrationBuilder.DropTable(
                name: "WagonModelFeatures");

            migrationBuilder.DropTable(
                name: "WagonFeatures");

            migrationBuilder.DropIndex(
                name: "IX_TrainWagons_CarrierId",
                table: "TrainWagons");

            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "TrainWagons");
        }
    }
}
