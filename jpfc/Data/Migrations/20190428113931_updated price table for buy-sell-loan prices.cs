using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class updatedpricetableforbuysellloanprices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Price",
                newName: "SellPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "BuyPrice",
                table: "Price",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanPrice",
                table: "Price",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanPricePercent",
                table: "Price",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "LoanPrice",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "LoanPricePercent",
                table: "Price");

            migrationBuilder.RenameColumn(
                name: "SellPrice",
                table: "Price",
                newName: "Amount");
        }
    }
}
