using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionInCoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinTemplates_WithdrawOptionGroups_WithdrawOptionGroupId",
                table: "CoinTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_PromotionCoins_WithdrawOptionGroups_WithdrawOptionGroupId",
                table: "PromotionCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_PromotionCoins_PromotionCoinId",
                table: "WithdrawOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawOptions_PromotionCoinId",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_PromotionCoins_WithdrawOptionGroupId",
                table: "PromotionCoins");

            migrationBuilder.DropIndex(
                name: "IX_CoinTemplates_WithdrawOptionGroupId",
                table: "CoinTemplates");

            migrationBuilder.DropColumn(
                name: "PromotionCoinId",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "WithdrawEndpointTemplateId",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "WithdrawOptionGroupId",
                table: "CoinTemplates");

            migrationBuilder.RenameColumn(
                name: "WithdrawOptionId",
                table: "WithdrawOptions",
                newName: "FromTemplateId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "WithdrawEndpointTemplates",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TransactionTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TransactionStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "RewardSource",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "WithdrawOptionGroupId",
                table: "PromotionCoins",
                newName: "FromTemplateId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PromotionCoins",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PrizeTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PlayerSegmentActTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PlayerLogTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Jobs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "HubSettings",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Games",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Currencies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "CoinTemplates",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AccountTypes",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PromotionCoins",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "CoinTemplates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CoinTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoinTemplates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WithdrawOptionPromotionCoinMappings",
                columns: table => new
                {
                    PromotionCoinId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionPromotionCoinMappings", x => new { x.PromotionCoinId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionPromotionCoinMappings_PromotionCoins_Promotio~",
                        column: x => x.PromotionCoinId,
                        principalTable: "PromotionCoins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionPromotionCoinMappings_WithdrawOptions_Withdra~",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionPromotionCoinMappings_WithdrawOptionId",
                table: "WithdrawOptionPromotionCoinMappings",
                column: "WithdrawOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawOptionPromotionCoinMappings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PromotionCoins");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "CoinTemplates");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CoinTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoinTemplates");

            migrationBuilder.RenameColumn(
                name: "FromTemplateId",
                table: "WithdrawOptions",
                newName: "WithdrawOptionId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WithdrawEndpointTemplates",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TransactionTypes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TransactionStatuses",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "RewardSource",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PromotionCoins",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "FromTemplateId",
                table: "PromotionCoins",
                newName: "WithdrawOptionGroupId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PrizeTypes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PlayerSegmentActTypes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PlayerLogTypes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Jobs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "HubSettings",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Games",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Currencies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CoinTemplates",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AccountTypes",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "PromotionCoinId",
                table: "WithdrawOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawOptionGroupId",
                table: "CoinTemplates",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptions_PromotionCoinId",
                table: "WithdrawOptions",
                column: "PromotionCoinId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions",
                column: "WithdrawOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoins_WithdrawOptionGroupId",
                table: "PromotionCoins",
                column: "WithdrawOptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinTemplates_WithdrawOptionGroupId",
                table: "CoinTemplates",
                column: "WithdrawOptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTemplates_WithdrawOptionGroups_WithdrawOptionGroupId",
                table: "CoinTemplates",
                column: "WithdrawOptionGroupId",
                principalTable: "WithdrawOptionGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCoins_WithdrawOptionGroups_WithdrawOptionGroupId",
                table: "PromotionCoins",
                column: "WithdrawOptionGroupId",
                principalTable: "WithdrawOptionGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_PromotionCoins_PromotionCoinId",
                table: "WithdrawOptions",
                column: "PromotionCoinId",
                principalTable: "PromotionCoins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId",
                principalTable: "WithdrawEndpointTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions",
                column: "WithdrawOptionId",
                principalTable: "WithdrawOptions",
                principalColumn: "Id");
        }
    }
}
