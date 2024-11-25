using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_CorellationId_Promotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews");

            migrationBuilder.DropTable(
                name: "PromotionCoinWithdrawOption");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionViewTemplateId",
                table: "PromotionViews",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "Correlationid",
                table: "Promotions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews",
                column: "PromotionViewTemplateId",
                principalTable: "PromotionViewTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews");

            migrationBuilder.DropTable(
                name: "WithdrawOptionPromotionCoinMappings");

            migrationBuilder.DropColumn(
                name: "Correlationid",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionViewTemplateId",
                table: "PromotionViews",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PromotionCoinWithdrawOption",
                columns: table => new
                {
                    PromotionCoinsId = table.Column<string>(type: "text", nullable: false),
                    WithdrawOptionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCoinWithdrawOption", x => new { x.PromotionCoinsId, x.WithdrawOptionsId });
                    table.ForeignKey(
                        name: "FK_PromotionCoinWithdrawOption_PromotionCoins_PromotionCoinsId",
                        column: x => x.PromotionCoinsId,
                        principalTable: "PromotionCoins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionCoinWithdrawOption_WithdrawOptions_WithdrawOptions~",
                        column: x => x.WithdrawOptionsId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoinWithdrawOption_WithdrawOptionsId",
                table: "PromotionCoinWithdrawOption",
                column: "WithdrawOptionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                table: "PromotionViews",
                column: "PromotionViewTemplateId",
                principalTable: "PromotionViewTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
