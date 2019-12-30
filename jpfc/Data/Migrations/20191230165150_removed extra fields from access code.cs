using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class removedextrafieldsfromaccesscode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "AccessCode");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "AccessCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "AccessCode",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "AccessCode",
                nullable: true);
        }
    }
}
