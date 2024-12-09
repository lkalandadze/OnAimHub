using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wheel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSegments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "JackpotPrizeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sequence = table.Column<List<int>>(type: "integer[]", nullable: false),
                    NextPrizeIndex = table.Column<int>(type: "integer", nullable: true),
                    ConfigurationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JackpotPrizeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrizeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsMultiplied = table.Column<bool>(type: "boolean", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrizeTypes_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Coins",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true),
                    WheelConfigurationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Coins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prices_GameConfigurations_WheelConfigurationId",
                        column: x => x.WheelConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Sequence = table.Column<List<int>>(type: "integer[]", nullable: false),
                    NextPrizeIndex = table.Column<int>(type: "integer", nullable: true),
                    ConfigurationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_GameConfigurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "GameConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JackpotPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Probability = table.Column<int>(type: "integer", nullable: false),
                    PrizeTypeId = table.Column<int>(type: "integer", nullable: false),
                    PrizeGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JackpotPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JackpotPrizes_JackpotPrizeGroups_PrizeGroupId",
                        column: x => x.PrizeGroupId,
                        principalTable: "JackpotPrizeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JackpotPrizes_PrizeTypes_PrizeTypeId",
                        column: x => x.PrizeTypeId,
                        principalTable: "PrizeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WheelPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    WheelIndex = table.Column<int>(type: "integer", nullable: true),
                    RoundId = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Probability = table.Column<int>(type: "integer", nullable: false),
                    PrizeTypeId = table.Column<int>(type: "integer", nullable: false),
                    PrizeGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WheelPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WheelPrizes_PrizeTypes_PrizeTypeId",
                        column: x => x.PrizeTypeId,
                        principalTable: "PrizeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WheelPrizes_Rounds_PrizeGroupId",
                        column: x => x.PrizeGroupId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JackpotPrizes_PrizeGroupId",
                table: "JackpotPrizes",
                column: "PrizeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JackpotPrizes_PrizeTypeId",
                table: "JackpotPrizes",
                column: "PrizeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CurrencyId",
                table: "Prices",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_WheelConfigurationId",
                table: "Prices",
                column: "WheelConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeTypes_CurrencyId",
                table: "PrizeTypes",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_ConfigurationId",
                table: "Rounds",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_WheelPrizes_PrizeGroupId",
                table: "WheelPrizes",
                column: "PrizeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WheelPrizes_PrizeTypeId",
                table: "WheelPrizes",
                column: "PrizeTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSettings");

            migrationBuilder.DropTable(
                name: "JackpotPrizes");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "WheelPrizes");

            migrationBuilder.DropTable(
                name: "JackpotPrizeGroups");

            migrationBuilder.DropTable(
                name: "PrizeTypes");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "GameConfigurations");
        }
    }
}
