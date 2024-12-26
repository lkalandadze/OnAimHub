using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_ContextId_ContextType_In_AggregationConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContextId",
                table: "AggregationConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContextType",
                table: "AggregationConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContextId",
                table: "AggregationConfigurations");

            migrationBuilder.DropColumn(
                name: "ContextType",
                table: "AggregationConfigurations");
        }
    }
}
