using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ticketing.Migrations
{
    public partial class Workflows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Input = table.Column<string>(type: "jsonb", nullable: true),
                    Output = table.Column<string>(type: "jsonb", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    MaxRetries = table.Column<int>(type: "integer", nullable: false),
                    ScheduledStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Context = table.Column<string>(type: "jsonb", nullable: true),
                    ParentTaskId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowTasks_WorkflowTasks_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTaskLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: true),
                    CallStack = table.Column<string>(type: "text", nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: true),
                    TaskId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTaskLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskLogs_WorkflowTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTaskProgresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Percent = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: true),
                    TaskId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTaskProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskProgresses_WorkflowTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskLogs_TaskId",
                table: "WorkflowTaskLogs",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskProgresses_TaskId",
                table: "WorkflowTaskProgresses",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_ParentTaskId",
                table: "WorkflowTasks",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_UserId",
                table: "WorkflowTasks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowTaskLogs");

            migrationBuilder.DropTable(
                name: "WorkflowTaskProgresses");

            migrationBuilder.DropTable(
                name: "WorkflowTasks");
        }
    }
}
