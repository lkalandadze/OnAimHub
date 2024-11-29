using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionViewTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawOptionPromotionCoinMappings");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Promotions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Promotions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionCoinWithdrawOption_WithdrawOptions_WithdrawOptions~",
                        column: x => x.WithdrawOptionsId,
                        principalTable: "WithdrawOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "PromotionViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    PromotionViewTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionViews_PromotionViewTemplates_PromotionViewTemplate~",
                        column: x => x.PromotionViewTemplateId,
                        principalTable: "PromotionViewTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionViews_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCoinWithdrawOption_WithdrawOptionsId",
                table: "PromotionCoinWithdrawOption",
                column: "WithdrawOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionViews_PromotionId",
                table: "PromotionViews",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionViews_PromotionViewTemplateId",
                table: "PromotionViews",
                column: "PromotionViewTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionCoinWithdrawOption");

            migrationBuilder.DropTable(
                name: "PromotionViews");

            migrationBuilder.DropTable(
                name: "PromotionViewTemplates");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Promotions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Promotions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionPromotionCoinMappings_WithdrawOptionId",
                table: "WithdrawOptionPromotionCoinMappings",
                column: "WithdrawOptionId");
        }
    }
}
