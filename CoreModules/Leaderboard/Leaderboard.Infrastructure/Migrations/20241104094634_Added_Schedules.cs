using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Schedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "LeaderboardRecords");

            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "LeaderboardTemplate",
                newName: "StartIn");

            migrationBuilder.RenameColumn(
                name: "CreationLeadTimeInDays",
                table: "LeaderboardTemplate",
                newName: "EndIn");

            migrationBuilder.RenameColumn(
                name: "AnnouncementLeadTimeInDays",
                table: "LeaderboardTemplate",
                newName: "AnnounceIn");

            migrationBuilder.CreateTable(
                name: "LeaderboardSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RepeatType = table.Column<int>(type: "integer", nullable: false),
                    RepeatValue = table.Column<int>(type: "integer", nullable: true),
                    SpecificDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LeaderboardTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardSchedules_LeaderboardTemplate_LeaderboardTemplat~",
                        column: x => x.LeaderboardTemplateId,
                        principalTable: "LeaderboardTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardSchedules_LeaderboardTemplateId",
                table: "LeaderboardSchedules",
                column: "LeaderboardTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderboardSchedules");

            migrationBuilder.RenameColumn(
                name: "StartIn",
                table: "LeaderboardTemplate",
                newName: "DurationInDays");

            migrationBuilder.RenameColumn(
                name: "EndIn",
                table: "LeaderboardTemplate",
                newName: "CreationLeadTimeInDays");

            migrationBuilder.RenameColumn(
                name: "AnnounceIn",
                table: "LeaderboardTemplate",
                newName: "AnnouncementLeadTimeInDays");

            migrationBuilder.AddColumn<int>(
                name: "JobType",
                table: "LeaderboardRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
