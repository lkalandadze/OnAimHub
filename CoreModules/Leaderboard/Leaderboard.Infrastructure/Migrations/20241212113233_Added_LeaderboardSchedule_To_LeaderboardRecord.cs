using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_LeaderboardSchedule_To_LeaderboardRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaderboardSchedules_LeaderboardRecordId",
                table: "LeaderboardSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardSchedules_LeaderboardRecordId",
                table: "LeaderboardSchedules",
                column: "LeaderboardRecordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaderboardSchedules_LeaderboardRecordId",
                table: "LeaderboardSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardSchedules_LeaderboardRecordId",
                table: "LeaderboardSchedules",
                column: "LeaderboardRecordId");
        }
    }
}
