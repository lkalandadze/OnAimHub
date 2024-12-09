using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_DurationsToSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementDurationHours",
                table: "LeaderboardSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreationHours",
                table: "LeaderboardSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndDurationHours",
                table: "LeaderboardSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartDurationHours",
                table: "LeaderboardSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementDurationHours",
                table: "LeaderboardSchedules");

            migrationBuilder.DropColumn(
                name: "CreationHours",
                table: "LeaderboardSchedules");

            migrationBuilder.DropColumn(
                name: "EndDurationHours",
                table: "LeaderboardSchedules");

            migrationBuilder.DropColumn(
                name: "StartDurationHours",
                table: "LeaderboardSchedules");
        }
    }
}
