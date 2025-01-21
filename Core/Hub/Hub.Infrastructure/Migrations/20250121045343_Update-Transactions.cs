using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Games_GameId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_GameId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Transactions",
                newName: "KeyId");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "WithdrawOptions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SourceServiceName",
                table: "Transactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AssetCoin_Value",
                table: "Coins",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OutCoin_Value",
                table: "Coins",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "WithdrawOptions");

            migrationBuilder.DropColumn(
                name: "SourceServiceName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AssetCoin_Value",
                table: "Coins");

            migrationBuilder.DropColumn(
                name: "OutCoin_Value",
                table: "Coins");

            migrationBuilder.RenameColumn(
                name: "KeyId",
                table: "Transactions",
                newName: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GameId",
                table: "Transactions",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Games_GameId",
                table: "Transactions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
