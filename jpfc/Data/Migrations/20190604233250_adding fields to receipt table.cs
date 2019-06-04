using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addingfieldstoreceipttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidInterestOnly",
                table: "ClientReceipt",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "ClientReceipt",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidInterestOnly",
                table: "ClientReceipt");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ClientReceipt");
        }
    }
}
