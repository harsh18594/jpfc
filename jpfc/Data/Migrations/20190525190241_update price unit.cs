using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class updatepriceunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(" UPDATE [Price] " +
                " SET [PerOunce] = 1 " +
                " WHERE [KaratId] = 'D9FB756F-933D-4CFA-9DC3-76714B84B256' ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
