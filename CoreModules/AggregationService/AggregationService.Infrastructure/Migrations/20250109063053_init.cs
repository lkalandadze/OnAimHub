using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AggregationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregationConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EventProducer = table.Column<string>(type: "text", nullable: false),
                    AggregationSubscriber = table.Column<string>(type: "text", nullable: false),
                    AggregationType = table.Column<int>(type: "integer", nullable: false),
                    EvaluationType = table.Column<int>(type: "integer", nullable: false),
                    SelectionField = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PromotionId = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregationConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Property = table.Column<string>(type: "text", nullable: false),
                    Operator = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    AggregationConfigurationId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filters_AggregationConfigurations_AggregationConfigurationId",
                        column: x => x.AggregationConfigurationId,
                        principalTable: "AggregationConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PointEvaluationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    Point = table.Column<int>(type: "integer", nullable: false),
                    AggregationConfigurationId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointEvaluationRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointEvaluationRules_AggregationConfigurations_AggregationC~",
                        column: x => x.AggregationConfigurationId,
                        principalTable: "AggregationConfigurations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filters_AggregationConfigurationId",
                table: "Filters",
                column: "AggregationConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PointEvaluationRules_AggregationConfigurationId",
                table: "PointEvaluationRules",
                column: "AggregationConfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "PointEvaluationRules");

            migrationBuilder.DropTable(
                name: "AggregationConfigurations");
        }
    }
}
