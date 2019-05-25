using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addingapriceunitflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PerOunce",
                table: "Price",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerOunce",
                table: "Price");
        }
    }
}
