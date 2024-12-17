using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Rewards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardPrizes_PrizeTypes_PrizeTypeId",
                table: "RewardPrizes");

            migrationBuilder.RenameColumn(
                name: "PrizeTypeId",
                table: "RewardPrizes",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_RewardPrizes_PrizeTypeId",
                table: "RewardPrizes",
                newName: "IX_RewardPrizes_CoinId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "WithdrawOptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WithdrawOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "WithdrawOptionGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WithdrawOptionGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "WithdrawOptionEndpoints",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WithdrawOptionEndpoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimableByPlayer",
                table: "Rewards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_RewardPrizes_Coins_CoinId",
                table: "RewardPrizes",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardPrizes_Coins_CoinId",
                table: "RewardPrizes");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "WithdrawOptionGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WithdrawOptionGroups");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "WithdrawOptionEndpoints");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WithdrawOptionEndpoints");

            migrationBuilder.DropColumn(
                name: "IsClaimableByPlayer",
                table: "Rewards");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "RewardPrizes",
                newName: "PrizeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RewardPrizes_CoinId",
                table: "RewardPrizes",
                newName: "IX_RewardPrizes_PrizeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardPrizes_PrizeTypes_PrizeTypeId",
                table: "RewardPrizes",
                column: "PrizeTypeId",
                principalTable: "PrizeTypes",
                principalColumn: "Id");
        }
    }
}
