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
                    AnnouncementLeadTimeInDays = table.Column<int>(type: "integer", nullable: false),
                    CreationLeadTimeInDays = table.Column<int>(type: "integer", nullable: false)
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
                    LeaderboardTemplateId = table.Column<int>(type: "integer", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AnnouncementDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LeaderboardType = table.Column<int>(type: "integer", nullable: false),
                    JobType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsGenerated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardRecords_LeaderboardTemplate_LeaderboardTemplateId",
                        column: x => x.LeaderboardTemplateId,
                        principalTable: "LeaderboardTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardTemplatePrize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaderboardTemplateId = table.Column<int>(type: "integer", nullable: false),
                    StartRank = table.Column<int>(type: "integer", nullable: false),
                    EndRank = table.Column<int>(type: "integer", nullable: false),
                    PrizeId = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardTemplatePrize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardTemplatePrize_LeaderboardTemplate_LeaderboardTem~",
                        column: x => x.LeaderboardTemplateId,
                        principalTable: "LeaderboardTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardTemplatePrize_Prize_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prize",
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
                    PrizeId = table.Column<string>(type: "text", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_LeaderboardRecordPrizes_Prize_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecordPrizes_LeaderboardRecordId",
                table: "LeaderboardRecordPrizes",
                column: "LeaderboardRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecordPrizes_PrizeId",
                table: "LeaderboardRecordPrizes",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRecords_LeaderboardTemplateId",
                table: "LeaderboardRecords",
                column: "LeaderboardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardTemplatePrize_LeaderboardTemplateId",
                table: "LeaderboardTemplatePrize",
                column: "LeaderboardTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardTemplatePrize_PrizeId",
                table: "LeaderboardTemplatePrize",
                column: "PrizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "LeaderboardRecordPrizes");

            migrationBuilder.DropTable(
                name: "LeaderboardTemplatePrize");

            migrationBuilder.DropTable(
                name: "LeaderboardRecords");

            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropTable(
                name: "LeaderboardTemplate");
        }
    }
}
