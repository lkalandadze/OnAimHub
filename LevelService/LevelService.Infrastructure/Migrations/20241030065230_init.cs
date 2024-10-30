using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LevelService.Infrastructure.Migrations
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
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DateFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DateTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsExpirable = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrizeTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrizeTypes_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ExperienceToArchieve = table.Column<int>(type: "integer", nullable: false),
                    ActId = table.Column<int>(type: "integer", nullable: false),
                    StageId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Levels_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LevelPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    PrizeTypeId = table.Column<string>(type: "text", nullable: false),
                    PrizeDeliveryType = table.Column<int>(type: "integer", nullable: false),
                    RankId = table.Column<int>(type: "integer", nullable: false),
                    LevelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelPrizes_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LevelPrizes_PrizeTypes_PrizeTypeId",
                        column: x => x.PrizeTypeId,
                        principalTable: "PrizeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelPrizes_LevelId",
                table: "LevelPrizes",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelPrizes_PrizeTypeId",
                table: "LevelPrizes",
                column: "PrizeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_StageId",
                table: "Levels",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeTypes_CurrencyId",
                table: "PrizeTypes",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelPrizes");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "PrizeTypes");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
