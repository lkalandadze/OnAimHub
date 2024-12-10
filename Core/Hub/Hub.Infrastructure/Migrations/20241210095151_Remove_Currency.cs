using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Currency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coins_CoinTemplates_CoinTemplateId",
                table: "Coins");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerBalances_Currencies_CurrencyId",
                table: "PlayerBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgresses_Currencies_CurrencyId",
                table: "PlayerProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgressHistories_Currencies_CurrencyId",
                table: "PlayerProgressHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTypes_Currencies_CurrencyId",
                table: "PrizeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDistributions_Currencies_ReferralPrizeCurrencyId",
                table: "ReferralDistributions");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDistributions_Currencies_ReferrerPrizeCurrencyId",
                table: "ReferralDistributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currencies_CurrencyId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "PromotionViewTemplates");

            migrationBuilder.DropTable(
                name: "WithdrawEndpointTemplates");

            migrationBuilder.DropTable(
                name: "WithdrawOptionCoinTemplateMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionPromotionCoinMappings");

            migrationBuilder.DropTable(
                name: "CoinTemplates");

            migrationBuilder.DropIndex(
                name: "IX_PromotionViews_PromotionViewTemplateId",
                table: "PromotionViews");

            migrationBuilder.DropIndex(
                name: "IX_Coins_CoinTemplateId",
                table: "Coins");

            migrationBuilder.DropColumn(
                name: "CoinTemplateId",
                table: "Coins");

            migrationBuilder.RenameColumn(
                name: "WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                newName: "WithdrawOptionEndpointId");

            migrationBuilder.RenameIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                newName: "IX_WithdrawOptions_WithdrawOptionEndpointId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Transactions",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CurrencyId",
                table: "Transactions",
                newName: "IX_Transactions_CoinId");

            migrationBuilder.RenameColumn(
                name: "ReferrerPrizeCurrencyId",
                table: "ReferralDistributions",
                newName: "ReferrerPrizeCoinId");

            migrationBuilder.RenameColumn(
                name: "ReferralPrizeCurrencyId",
                table: "ReferralDistributions",
                newName: "ReferralPrizeCoinId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferralDistributions_ReferrerPrizeCurrencyId",
                table: "ReferralDistributions",
                newName: "IX_ReferralDistributions_ReferrerPrizeCoinId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferralDistributions_ReferralPrizeCurrencyId",
                table: "ReferralDistributions",
                newName: "IX_ReferralDistributions_ReferralPrizeCoinId");

            migrationBuilder.RenameColumn(
                name: "PromotionViewTemplateId",
                table: "PromotionViews",
                newName: "FromTemplateId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PrizeTypes",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTypes_CurrencyId",
                table: "PrizeTypes",
                newName: "IX_PrizeTypes_CoinId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PlayerProgressHistories",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgressHistories_CurrencyId",
                table: "PlayerProgressHistories",
                newName: "IX_PlayerProgressHistories_CoinId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PlayerProgresses",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgresses_CurrencyId",
                table: "PlayerProgresses",
                newName: "IX_PlayerProgresses_CoinId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PlayerBalances",
                newName: "CoinId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerBalances_CurrencyId",
                table: "PlayerBalances",
                newName: "IX_PlayerBalances_CoinId");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Jobs",
                newName: "CoinId");

            migrationBuilder.AddColumn<int>(
                name: "PriorityIndex",
                table: "WithdrawOptionGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromTemplateId",
                table: "Promotions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromTemplateId",
                table: "Coins",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoinWithdrawOptionGroupMappings",
                columns: table => new
                {
                    CoinId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinWithdrawOptionGroupMappings", x => new { x.CoinId, x.WithdrawOptionGroupId });
                    table.ForeignKey(
                        name: "FK_CoinWithdrawOptionGroupMappings_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoinWithdrawOptionGroupMappings_WithdrawOptionGroups_Withdr~",
                        column: x => x.WithdrawOptionGroupId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoinWithdrawOptionMappings",
                columns: table => new
                {
                    CoinId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinWithdrawOptionMappings", x => new { x.CoinId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_CoinWithdrawOptionMappings_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoinWithdrawOptionMappings_WithdrawOptions_WithdrawOptionId",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionEndpoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionEndpoints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinWithdrawOptionGroupMappings_WithdrawOptionGroupId",
                table: "CoinWithdrawOptionGroupMappings",
                column: "WithdrawOptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinWithdrawOptionMappings_WithdrawOptionId",
                table: "CoinWithdrawOptionMappings",
                column: "WithdrawOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerBalances_Coins_CoinId",
                table: "PlayerBalances",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgresses_Coins_CoinId",
                table: "PlayerProgresses",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgressHistories_Coins_CoinId",
                table: "PlayerProgressHistories",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTypes_Coins_CoinId",
                table: "PrizeTypes",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDistributions_Coins_ReferralPrizeCoinId",
                table: "ReferralDistributions",
                column: "ReferralPrizeCoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDistributions_Coins_ReferrerPrizeCoinId",
                table: "ReferralDistributions",
                column: "ReferrerPrizeCoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Coins_CoinId",
                table: "Transactions",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_WithdrawOptionEndpoints_WithdrawOptionEndpo~",
                table: "WithdrawOptions",
                column: "WithdrawOptionEndpointId",
                principalTable: "WithdrawOptionEndpoints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerBalances_Coins_CoinId",
                table: "PlayerBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgresses_Coins_CoinId",
                table: "PlayerProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgressHistories_Coins_CoinId",
                table: "PlayerProgressHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTypes_Coins_CoinId",
                table: "PrizeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDistributions_Coins_ReferralPrizeCoinId",
                table: "ReferralDistributions");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralDistributions_Coins_ReferrerPrizeCoinId",
                table: "ReferralDistributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Coins_CoinId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_WithdrawOptionEndpoints_WithdrawOptionEndpo~",
                table: "WithdrawOptions");

            migrationBuilder.DropTable(
                name: "CoinWithdrawOptionGroupMappings");

            migrationBuilder.DropTable(
                name: "CoinWithdrawOptionMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionEndpoints");

            migrationBuilder.DropColumn(
                name: "PriorityIndex",
                table: "WithdrawOptionGroups");

            migrationBuilder.DropColumn(
                name: "FromTemplateId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "FromTemplateId",
                table: "Coins");

            migrationBuilder.RenameColumn(
                name: "WithdrawOptionEndpointId",
                table: "WithdrawOptions",
                newName: "WithdrawEndpointTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_WithdrawOptions_WithdrawOptionEndpointId",
                table: "WithdrawOptions",
                newName: "IX_WithdrawOptions_WithdrawEndpointTemplateId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "Transactions",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CoinId",
                table: "Transactions",
                newName: "IX_Transactions_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "ReferrerPrizeCoinId",
                table: "ReferralDistributions",
                newName: "ReferrerPrizeCurrencyId");

            migrationBuilder.RenameColumn(
                name: "ReferralPrizeCoinId",
                table: "ReferralDistributions",
                newName: "ReferralPrizeCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferralDistributions_ReferrerPrizeCoinId",
                table: "ReferralDistributions",
                newName: "IX_ReferralDistributions_ReferrerPrizeCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ReferralDistributions_ReferralPrizeCoinId",
                table: "ReferralDistributions",
                newName: "IX_ReferralDistributions_ReferralPrizeCurrencyId");

            migrationBuilder.RenameColumn(
                name: "FromTemplateId",
                table: "PromotionViews",
                newName: "PromotionViewTemplateId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "PrizeTypes",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTypes_CoinId",
                table: "PrizeTypes",
                newName: "IX_PrizeTypes_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "PlayerProgressHistories",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgressHistories_CoinId",
                table: "PlayerProgressHistories",
                newName: "IX_PlayerProgressHistories_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "PlayerProgresses",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgresses_CoinId",
                table: "PlayerProgresses",
                newName: "IX_PlayerProgresses_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "PlayerBalances",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerBalances_CoinId",
                table: "PlayerBalances",
                newName: "IX_PlayerBalances_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "CoinId",
                table: "Jobs",
                newName: "CurrencyId");

            migrationBuilder.AddColumn<int>(
                name: "CoinTemplateId",
                table: "Coins",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoinTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoinType = table.Column<int>(type: "integer", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinTemplates", x => x.Id);
                });

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
                name: "PromotionViewTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionViewTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawEndpointTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    EndpointContent = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawEndpointTemplates", x => x.Id);
                });

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
                        name: "FK_WithdrawOptionPromotionCoinMappings_Coins_PromotionCoinId",
                        column: x => x.PromotionCoinId,
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionPromotionCoinMappings_WithdrawOptions_Withdra~",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionCoinTemplateMappings",
                columns: table => new
                {
                    CoinTemplateId = table.Column<int>(type: "integer", nullable: false),
                    WithdrawOptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionCoinTemplateMappings", x => new { x.CoinTemplateId, x.WithdrawOptionId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_CoinTemplates_CoinTempla~",
                        column: x => x.CoinTemplateId,
                        principalTable: "CoinTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptions_Withdraw~",
                        column: x => x.WithdrawOptionId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionViews_PromotionViewTemplateId",
                table: "PromotionViews",
                column: "PromotionViewTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Coins_CoinTemplateId",
                table: "Coins",
                column: "CoinTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionId",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionPromotionCoinMappings_WithdrawOptionId",
                table: "WithdrawOptionPromotionCoinMappings",
                column: "WithdrawOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coins_CoinTemplates_CoinTemplateId",
                table: "Coins",
                column: "CoinTemplateId",
                principalTable: "CoinTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerBalances_Currencies_CurrencyId",
                table: "PlayerBalances",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgresses_Currencies_CurrencyId",
                table: "PlayerProgresses",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgressHistories_Currencies_CurrencyId",
                table: "PlayerProgressHistories",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTypes_Currencies_CurrencyId",
                table: "PrizeTypes",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews",
                column: "PromotionViewTemplateId",
                principalTable: "PromotionViewTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDistributions_Currencies_ReferralPrizeCurrencyId",
                table: "ReferralDistributions",
                column: "ReferralPrizeCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralDistributions_Currencies_ReferrerPrizeCurrencyId",
                table: "ReferralDistributions",
                column: "ReferrerPrizeCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currencies_CurrencyId",
                table: "Transactions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId",
                principalTable: "WithdrawEndpointTemplates",
                principalColumn: "Id");
        }
    }
}
