using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderboardRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AnnouncementDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: true),
                    ScheduleId = table.Column<int>(type: "integer", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaderboardRecordId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    PlayerUsername = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardProgresses_LeaderboardRecords_LeaderboardRecordId",
                        column: x => x.LeaderboardRecordId,
                        principalTable: "LeaderboardRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardRecordPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaderboardRecordId = table.Column<int>(type: "integer", nullable: false),
                    StartRank = table.Column<int>(type: "integer", nullable: false),
                    EndRank = table.Column<int>(type: "integer", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRecordPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardRecordPrizes_LeaderboardRecords_LeaderboardRecor~",
                        column: x => x.LeaderboardRecordId,
                        principalTable: "LeaderboardRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaderboardRecordId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    PlayerUsername = table.Column<string>(type: "text", nullable: false),
                    Placement = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardResults_LeaderboardRecords_LeaderboardRecordId",
                        column: x => x.LeaderboardRecordId,
                        principalTable: "LeaderboardRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    RepeatType = table.Column<int>(type: "integer", nullable: false),
                    RepeatValue = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LeaderboardRecordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardSchedules_LeaderboardRecords_LeaderboardRecordId",
                        column: x => x.LeaderboardRecordId,
                        principalTable: "LeaderboardRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardProgresses_LeaderboardRecordId",
                table: "LeaderboardProgresses",
                column: "LeaderboardRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecordPrizes_LeaderboardRecordId",
                table: "LeaderboardRecordPrizes",
                column: "LeaderboardRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardResults_LeaderboardRecordId",
                table: "LeaderboardResults",
                column: "LeaderboardRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardSchedules_LeaderboardRecordId",
                table: "LeaderboardSchedules",
                column: "LeaderboardRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderboardProgresses");

            migrationBuilder.DropTable(
                name: "LeaderboardRecordPrizes");

            migrationBuilder.DropTable(
                name: "LeaderboardResults");

            migrationBuilder.DropTable(
                name: "LeaderboardSchedules");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "LeaderboardRecords");
        }
    }
}
