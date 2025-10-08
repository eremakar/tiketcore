using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class TrainType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Trains",
                newName: "Importance");

            migrationBuilder.AddColumn<bool>(
                name: "HasLiftingMechanism",
                table: "WagonModels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerName",
                table: "WagonModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Amenities",
                table: "Trains",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PeriodicityId",
                table: "Trains",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Trains",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Carriers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Filial",
                table: "Carriers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periodicities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodicities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trains_PeriodicityId",
                table: "Trains",
                column: "PeriodicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TypeId",
                table: "Trains",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Periodicities_PeriodicityId",
                table: "Trains",
                column: "PeriodicityId",
                principalTable: "Periodicities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainTypes_TypeId",
                table: "Trains",
                column: "TypeId",
                principalTable: "TrainTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Periodicities_PeriodicityId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainTypes_TypeId",
                table: "Trains");

            migrationBuilder.DropTable(
                name: "Filials");

            migrationBuilder.DropTable(
                name: "Periodicities");

            migrationBuilder.DropTable(
                name: "TrainTypes");

            migrationBuilder.DropIndex(
                name: "IX_Trains_PeriodicityId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TypeId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "HasLiftingMechanism",
                table: "WagonModels");

            migrationBuilder.DropColumn(
                name: "ManufacturerName",
                table: "WagonModels");

            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "PeriodicityId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "Filial",
                table: "Carriers");

            migrationBuilder.RenameColumn(
                name: "Importance",
                table: "Trains",
                newName: "Type");
        }
    }
}
