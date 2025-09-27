using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Railwaies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ShortCode = table.Column<string>(type: "text", nullable: true),
                    TimeDifferenceFromAdministration = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Railwaies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    ShortNameLatin = table.Column<string>(type: "text", nullable: true),
                    Depots = table.Column<string>(type: "jsonb", nullable: true),
                    IsCity = table.Column<bool>(type: "boolean", nullable: false),
                    CityCode = table.Column<string>(type: "text", nullable: true),
                    IsSalePoint = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wagons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true),
                    SeatCount = table.Column<int>(type: "integer", nullable: false),
                    PictureS3 = table.Column<string>(type: "jsonb", nullable: true),
                    Class = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wagons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ProtectFromBruteforceAttempts = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PositionName = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    RegistrationToken = table.Column<string>(type: "text", nullable: true),
                    BlockExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PushToken = table.Column<string>(type: "text", nullable: true),
                    SignalrToken = table.Column<string>(type: "text", nullable: true),
                    PinCode = table.Column<string>(type: "text", nullable: true),
                    PinCodeExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Avatar = table.Column<string>(type: "jsonb", nullable: true),
                    FailedLoginCount = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Connections_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depots_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RailwayStations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StationId = table.Column<long>(type: "bigint", nullable: true),
                    RailwayId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RailwayStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RailwayStations_Railwaies_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Railwaies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RailwayStations_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Class = table.Column<int>(type: "integer", nullable: false),
                    WagonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Wagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "Wagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteStations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Arrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Stop = table.Column<bool>(type: "boolean", nullable: false),
                    Departure = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StationId = table.Column<long>(type: "bigint", nullable: true),
                    RouteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStations_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteStations_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ZoneType = table.Column<int>(type: "integer", nullable: false),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    RouteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trains_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trains_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trains_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainSchedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    TrainId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainSchedules_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainWagons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainWagons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainWagons_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainWagons_Wagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "Wagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeatCountReservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<string>(type: "text", nullable: true),
                    SeatCount = table.Column<int>(type: "integer", nullable: false),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatCountReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatCountReservations_RouteStations_FromId",
                        column: x => x.FromId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountReservations_RouteStations_ToId",
                        column: x => x.ToId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountReservations_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountReservations_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountReservations_TrainWagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "TrainWagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeatCountSegments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeatCount = table.Column<int>(type: "integer", nullable: false),
                    FreeCount = table.Column<int>(type: "integer", nullable: false),
                    Tickets = table.Column<string>(type: "jsonb", nullable: true),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatCountSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatCountSegments_RouteStations_FromId",
                        column: x => x.FromId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountSegments_RouteStations_ToId",
                        column: x => x.ToId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountSegments_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountSegments_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatCountSegments_TrainWagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "TrainWagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeatReservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<string>(type: "text", nullable: true),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    SeatId = table.Column<long>(type: "bigint", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatReservations_RouteStations_FromId",
                        column: x => x.FromId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatReservations_RouteStations_ToId",
                        column: x => x.ToId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatReservations_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatReservations_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatReservations_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatReservations_TrainWagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "TrainWagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<string>(type: "text", nullable: true),
                    IsSeat = table.Column<bool>(type: "boolean", nullable: false),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    SeatId = table.Column<long>(type: "bigint", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_RouteStations_FromId",
                        column: x => x.FromId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_RouteStations_ToId",
                        column: x => x.ToId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_TrainWagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "TrainWagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeatCountReservationSeatCountSegment",
                columns: table => new
                {
                    SeatCountReservationId = table.Column<long>(type: "bigint", nullable: false),
                    SegmentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatCountReservationSeatCountSegment", x => new { x.SeatCountReservationId, x.SegmentsId });
                    table.ForeignKey(
                        name: "FK_SeatCountReservationSeatCountSegment_SeatCountReservations_~",
                        column: x => x.SeatCountReservationId,
                        principalTable: "SeatCountReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeatCountReservationSeatCountSegment_SeatCountSegments_Segm~",
                        column: x => x.SegmentsId,
                        principalTable: "SeatCountSegments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatSegments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeatId = table.Column<long>(type: "bigint", nullable: true),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true),
                    TrainId = table.Column<long>(type: "bigint", nullable: true),
                    WagonId = table.Column<long>(type: "bigint", nullable: true),
                    TrainScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    TicketId = table.Column<long>(type: "bigint", nullable: true),
                    SeatReservationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatSegments_RouteStations_FromId",
                        column: x => x.FromId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_RouteStations_ToId",
                        column: x => x.ToId,
                        principalTable: "RouteStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_SeatReservations_SeatReservationId",
                        column: x => x.SeatReservationId,
                        principalTable: "SeatReservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_TrainSchedules_TrainScheduleId",
                        column: x => x.TrainScheduleId,
                        principalTable: "TrainSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SeatSegments_TrainWagons_WagonId",
                        column: x => x.WagonId,
                        principalTable: "TrainWagons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_FromId",
                table: "Connections",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ToId",
                table: "Connections",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_StationId",
                table: "Depots",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_RailwayStations_RailwayId",
                table: "RailwayStations",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_RailwayStations_StationId",
                table: "RailwayStations",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TrainId",
                table: "Routes",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStations_RouteId",
                table: "RouteStations",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStations_StationId",
                table: "RouteStations",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservations_FromId",
                table: "SeatCountReservations",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservations_ToId",
                table: "SeatCountReservations",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservations_TrainId",
                table: "SeatCountReservations",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservations_TrainScheduleId",
                table: "SeatCountReservations",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservations_WagonId",
                table: "SeatCountReservations",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountReservationSeatCountSegment_SegmentsId",
                table: "SeatCountReservationSeatCountSegment",
                column: "SegmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountSegments_FromId",
                table: "SeatCountSegments",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountSegments_ToId",
                table: "SeatCountSegments",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountSegments_TrainId",
                table: "SeatCountSegments",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountSegments_TrainScheduleId",
                table: "SeatCountSegments",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatCountSegments_WagonId",
                table: "SeatCountSegments",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_FromId",
                table: "SeatReservations",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_SeatId",
                table: "SeatReservations",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_ToId",
                table: "SeatReservations",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_TrainId",
                table: "SeatReservations",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_TrainScheduleId",
                table: "SeatReservations",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_WagonId",
                table: "SeatReservations",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_WagonId",
                table: "Seats",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_FromId",
                table: "SeatSegments",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_SeatId",
                table: "SeatSegments",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_SeatReservationId",
                table: "SeatSegments",
                column: "SeatReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_TicketId",
                table: "SeatSegments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_ToId",
                table: "SeatSegments",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_TrainId",
                table: "SeatSegments",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_TrainScheduleId",
                table: "SeatSegments",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatSegments_WagonId",
                table: "SeatSegments",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FromId",
                table: "Tickets",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ToId",
                table: "Tickets",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrainId",
                table: "Tickets",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrainScheduleId",
                table: "Tickets",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_WagonId",
                table: "Tickets",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_FromId",
                table: "Trains",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_RouteId",
                table: "Trains",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_ToId",
                table: "Trains",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainSchedules_TrainId",
                table: "TrainSchedules",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagons_TrainScheduleId",
                table: "TrainWagons",
                column: "TrainScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainWagons_WagonId",
                table: "TrainWagons",
                column: "WagonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Trains_TrainId",
                table: "Routes",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_FromId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_ToId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Trains_TrainId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "RailwayStations");

            migrationBuilder.DropTable(
                name: "SeatCountReservationSeatCountSegment");

            migrationBuilder.DropTable(
                name: "SeatSegments");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Railwaies");

            migrationBuilder.DropTable(
                name: "SeatCountReservations");

            migrationBuilder.DropTable(
                name: "SeatCountSegments");

            migrationBuilder.DropTable(
                name: "SeatReservations");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RouteStations");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "TrainWagons");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TrainSchedules");

            migrationBuilder.DropTable(
                name: "Wagons");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
