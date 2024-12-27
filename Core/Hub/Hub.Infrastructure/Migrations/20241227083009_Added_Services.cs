using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Services : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionServices");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "WithdrawOptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "WithdrawOptionGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "WithdrawOptionEndpoints",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "PromotionViews",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Promotions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionServiceMappings",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionServiceMappings", x => new { x.PromotionId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_PromotionServiceMappings_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionServiceMappings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServiceMappings_ServiceId",
                table: "PromotionServiceMappings",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionServiceMappings");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "WithdrawOptionGroups");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "WithdrawOptionEndpoints");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "PromotionViews");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Promotions");

            migrationBuilder.CreateTable(
                name: "PromotionServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Service = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionServices_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_PromotionId",
                table: "PromotionServices",
                column: "PromotionId");
        }
    }
}
