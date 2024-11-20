using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CoinTemplateRelationshipsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawOptionCoinTemplateMappings");

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

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionGroupCoinTemplateMappings_WithdrawOptionGroup~",
                table: "WithdrawOptionGroupCoinTemplateMappings",
                column: "WithdrawOptionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawOptionGroupCoinTemplateMappings");

            migrationBuilder.CreateTable(
                name: "WithdrawOptionCoinTemplateMappings",
                columns: table => new
                {
                    CoinTemplateId = table.Column<int>(type: "integer", nullable: false),
                    WithdrawOptionGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOptionCoinTemplateMappings", x => new { x.CoinTemplateId, x.WithdrawOptionGroupId });
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_CoinTemplates_CoinTempla~",
                        column: x => x.CoinTemplateId,
                        principalTable: "CoinTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroups_Wit~",
                        column: x => x.WithdrawOptionGroupId,
                        principalTable: "WithdrawOptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawOptionCoinTemplateMappings_WithdrawOptionGroupId",
                table: "WithdrawOptionCoinTemplateMappings",
                column: "WithdrawOptionGroupId");
        }
    }
}
