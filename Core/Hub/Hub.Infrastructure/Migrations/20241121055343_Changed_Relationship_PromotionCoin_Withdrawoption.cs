using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Relationship_PromotionCoin_Withdrawoption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawOptionGroupCoinTemplateMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionGroupPromotionCoinMappings");

            migrationBuilder.AddColumn<string>(
                name: "PromotionCoinId",
                table: "WithdrawOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawOptionId",
                table: "WithdrawOptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawOptionGroupId",
                table: "PromotionCoins",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawOptionGroupId",
                table: "CoinTemplates",
                type: "integer",
                nullable: true);

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
                name: "IX_WithdrawOptions_PromotionCoinId",
                table: "WithdrawOptions",
                column: "PromotionCoinId");

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

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionId",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionId");

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
                name: "FK_WithdrawOptions_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions",
                column: "WithdrawOptionId",
                principalTable: "WithdrawOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_WithdrawOptions_WithdrawOptions_WithdrawOptionId",
                table: "WithdrawOptions");

            migrationBuilder.DropTable(
                name: "WithdrawOptionCoinTemplateMappings");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawOptions_PromotionCoinId",
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
                name: "WithdrawOptionId",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "WithdrawOptionGroupId",
                table: "PromotionCoins");

            migrationBuilder.DropColumn(
                name: "WithdrawOptionGroupId",
                table: "CoinTemplates");

            migrationBuilder.CreateTable(
                name: "WithdrawOptionGroupCoinTemplateMappings",
                columns: table => new
                {
                    CoinTemplateId = table.Column<int>(type: "integer", nullable: false),
                    WithdrawOptionGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionGroupCoinTemplateMappings", x => new { x.CoinTemplateId, x.WithdrawOptionGroupId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupCoinTemplateMappings_CoinTemplates_CoinT~",
                        column: x => x.CoinTemplateId,
                        principalTable: "CoinTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupCoinTemplateMappings_WithdrawOptionGroup~",
                        column: x => x.WithdrawOptionGroupId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOptionGroupPromotionCoinMappings",
                columns: table => new
                {
                    PromotionCoinId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionGroupPromotionCoinMappings", x => new { x.PromotionCoinId, x.WithdrawOptionGroupId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupPromotionCoinMappings_PromotionCoins_Pro~",
                        column: x => x.PromotionCoinId,
                        principalTable: "PromotionCoins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionGroupPromotionCoinMappings_WithdrawOptionGrou~",
                        column: x => x.WithdrawOptionGroupId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionGroupCoinTemplateMappings_WithdrawOptionGroup~",
                table: "WithdrawOptionGroupCoinTemplateMappings",
                column: "WithdrawOptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionGroupPromotionCoinMappings_WithdrawOptionGrou~",
                table: "WithdrawOptionGroupPromotionCoinMappings",
                column: "WithdrawOptionGroupId");
        }
    }
}
