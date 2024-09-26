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
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DurationInDays = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementLeadTimeInDays = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LeaderboardTemplateId = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LeaderboardType = table.Column<int>(type: "integer", nullable: false),
                    JobType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardRecords_LeaderboardTemplate_LeaderboardTemplateId",
                        column: x => x.LeaderboardTemplateId,
                        principalTable: "LeaderboardTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartRank = table.Column<int>(type: "integer", nullable: false),
                    EndRank = table.Column<int>(type: "integer", nullable: false),
                    LeaderboardRecordId = table.Column<int>(type: "integer", nullable: true),
                    LeaderboardTemplateId = table.Column<int>(type: "integer", nullable: true),
                    PrizeId = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardPrizes_LeaderboardRecords_LeaderboardRecordId",
                        column: x => x.LeaderboardRecordId,
                        principalTable: "LeaderboardRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaderboardPrizes_LeaderboardTemplate_LeaderboardTemplateId",
                        column: x => x.LeaderboardTemplateId,
                        principalTable: "LeaderboardTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaderboardPrizes_Prize_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardPrizes_LeaderboardRecordId",
                table: "LeaderboardPrizes",
                column: "LeaderboardRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardPrizes_LeaderboardTemplateId",
                table: "LeaderboardPrizes",
                column: "LeaderboardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardPrizes_PrizeId",
                table: "LeaderboardPrizes",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecords_LeaderboardTemplateId",
                table: "LeaderboardRecords",
                column: "LeaderboardTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "LeaderboardPrizes");

            migrationBuilder.DropTable(
                name: "LeaderboardRecords");

            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropTable(
                name: "LeaderboardTemplate");
        }
    }
}
