using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelationsBetweenEntityAndTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FromTemplateId",
                table: "WithdrawOptions",
                newName: "WithdrawEndpointTemplateId");

            migrationBuilder.RenameColumn(
                name: "FromTemplateId",
                table: "PromotionCoins",
                newName: "CoinTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoins_CoinTemplateId",
                table: "PromotionCoins",
                column: "CoinTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionCoins_CoinTemplates_CoinTemplateId",
                table: "PromotionCoins",
                column: "CoinTemplateId",
                principalTable: "CoinTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions",
                column: "WithdrawEndpointTemplateId",
                principalTable: "WithdrawEndpointTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionCoins_CoinTemplates_CoinTemplateId",
                table: "PromotionCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawOptions_WithdrawEndpointTemplates_WithdrawEndpointT~",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawOptions_WithdrawEndpointTemplateId",
                table: "WithdrawOptions");

            migrationBuilder.DropIndex(
                name: "IX_PromotionCoins_CoinTemplateId",
                table: "PromotionCoins");

            migrationBuilder.RenameColumn(
                name: "WithdrawEndpointTemplateId",
                table: "WithdrawOptions",
                newName: "FromTemplateId");

            migrationBuilder.RenameColumn(
                name: "CoinTemplateId",
                table: "PromotionCoins",
                newName: "FromTemplateId");
        }
    }
}
