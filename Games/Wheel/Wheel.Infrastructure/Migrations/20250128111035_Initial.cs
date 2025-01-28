using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wheel.Infrastructure.Migrations
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
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: true),
                    WheelConfigurationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_GameConfigurations_WheelConfigurationId",
                        column: x => x.WheelConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WheelPrizeGroups",
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
                    table.PrimaryKey("PK_WheelPrizeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WheelPrizeGroups_GameConfigurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WheelPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WheelIndex = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_WheelPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WheelPrizes_WheelPrizeGroups_PrizeGroupId",
                        column: x => x.PrizeGroupId,
                        principalTable: "WheelPrizeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_WheelConfigurationId",
                table: "Prices",
                column: "WheelConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_WheelPrizeGroups_ConfigurationId",
                table: "WheelPrizeGroups",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_WheelPrizes_PrizeGroupId",
                table: "WheelPrizes",
                column: "PrizeGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSettings");

            migrationBuilder.DropTable(
                name: "LimitedPrizeCountsByPlayer");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "WheelPrizes");

            migrationBuilder.DropTable(
                name: "WheelPrizeGroups");

            migrationBuilder.DropTable(
                name: "GameConfigurations");
        }
    }
}
