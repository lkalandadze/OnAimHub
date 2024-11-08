using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Unused_DataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "LeaderboardTemplate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobType",
                table: "LeaderboardTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
