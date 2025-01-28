using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PenaltyKicks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KicksCount = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FromTemplateId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LimitedPrizeCountsByPlayer",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    PrizeId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitedPrizeCountsByPlayer", x => new { x.PlayerId, x.PrizeId });
                });

            migrationBuilder.CreateTable(
                name: "PenaltyGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    BetPriceId = table.Column<int>(type: "integer", nullable: false),
                    PrizeId = table.Column<int>(type: "integer", nullable: false),
                    PrizeValue = table.Column<int>(type: "integer", nullable: false),
                    PriceMultiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: true),
                    KickSequence = table.Column<List<bool>>(type: "boolean[]", nullable: true),
                    GoalsScored = table.Column<int>(type: "integer", nullable: false),
                    CurrentKickIndex = table.Column<int>(type: "integer", nullable: false),
                    GameState = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PenaltyPrizeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Sequence = table.Column<List<int>>(type: "integer[]", nullable: true),
                    NextPrizeIndex = table.Column<int>(type: "integer", nullable: true),
                    ConfigurationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyPrizeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PenaltyPrizeGroups_GameConfigurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: true),
                    PenaltyConfigurationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_GameConfigurations_PenaltyConfigurationId",
                        column: x => x.PenaltyConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PenaltyPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Probability = table.Column<int>(type: "integer", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: true),
                    PerPlayerLimit = table.Column<int>(type: "integer", nullable: true),
                    GlobalLimit = table.Column<int>(type: "integer", nullable: true),
                    RemainingGlobalLimit = table.Column<int>(type: "integer", nullable: true),
                    SetSize = table.Column<int>(type: "integer", nullable: true),
                    PerPlayerSetLimit = table.Column<int>(type: "integer", nullable: true),
                    GlobalSetLimit = table.Column<int>(type: "integer", nullable: true),
                    RemainingGlobalSetLimit = table.Column<int>(type: "integer", nullable: true),
                    PrizeGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PenaltyPrizes_PenaltyPrizeGroups_PrizeGroupId",
                        column: x => x.PrizeGroupId,
                        principalTable: "PenaltyPrizeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPrizeGroups_ConfigurationId",
                table: "PenaltyPrizeGroups",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PenaltyPrizes_PrizeGroupId",
                table: "PenaltyPrizes",
                column: "PrizeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PenaltyConfigurationId",
                table: "Prices",
                column: "PenaltyConfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSettings");

            migrationBuilder.DropTable(
                name: "LimitedPrizeCountsByPlayer");

            migrationBuilder.DropTable(
                name: "PenaltyGame");

            migrationBuilder.DropTable(
                name: "PenaltyPrizes");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "PenaltyPrizeGroups");

            migrationBuilder.DropTable(
                name: "GameConfigurations");
        }
    }
}
