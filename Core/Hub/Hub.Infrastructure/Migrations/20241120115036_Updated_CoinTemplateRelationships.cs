using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CoinTemplateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptions_Withdraw~",
                table: "WithdrawOptionCoinTemplateMappings");

            migrationBuilder.DropTable(
                name: "PromotionCoinWithdrawOptionGroup");

            migrationBuilder.RenameColumn(
                name: "WithdrawOptionId",
                table: "WithdrawOptionCoinTemplateMappings",
                newName: "WithdrawOptionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionId",
                table: "WithdrawOptionCoinTemplateMappings",
                newName: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroupId");

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
                name: "IX_WithdrawOptionGroupPromotionCoinMappings_WithdrawOptionGrou~",
                table: "WithdrawOptionGroupPromotionCoinMappings",
                column: "WithdrawOptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroups_Wit~",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionGroupId",
                principalTable: "WithdrawOptionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroups_Wit~",
                table: "WithdrawOptionCoinTemplateMappings");

            migrationBuilder.DropTable(
                name: "WithdrawOptionGroupPromotionCoinMappings");

            migrationBuilder.RenameColumn(
                name: "WithdrawOptionGroupId",
                table: "WithdrawOptionCoinTemplateMappings",
                newName: "WithdrawOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroupId",
                table: "WithdrawOptionCoinTemplateMappings",
                newName: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionId");

            migrationBuilder.CreateTable(
                name: "PromotionCoinWithdrawOptionGroup",
                columns: table => new
                {
                    PromotionCoinsId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionGroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCoinWithdrawOptionGroup", x => new { x.PromotionCoinsId, x.WithdrawOptionGroupsId });
                    table.ForeignKey(
                        name: "FK_PromotionCoinWithdrawOptionGroup_PromotionCoins_PromotionCo~",
                        column: x => x.PromotionCoinsId,
                        principalTable: "PromotionCoins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionCoinWithdrawOptionGroup_WithdrawOptionGroups_Withd~",
                        column: x => x.WithdrawOptionGroupsId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoinWithdrawOptionGroup_WithdrawOptionGroupsId",
                table: "PromotionCoinWithdrawOptionGroup",
                column: "WithdrawOptionGroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptions_Withdraw~",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionId",
                principalTable: "WithdrawOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
