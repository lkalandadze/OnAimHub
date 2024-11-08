using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LevelService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LevelGrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "Configuration",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "PlayerReward",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    LevelPrizeId = table.Column<int>(type: "integer", nullable: false),
                    RewardStatus = table.Column<int>(type: "integer", nullable: false),
                    DateClaimed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerReward_LevelPrizes_LevelPrizeId",
                        column: x => x.LevelPrizeId,
                        principalTable: "LevelPrizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerReward_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReward_LevelPrizeId",
                table: "PlayerReward",
                column: "LevelPrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReward_PlayerId",
                table: "PlayerReward",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerReward");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "Configuration",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
