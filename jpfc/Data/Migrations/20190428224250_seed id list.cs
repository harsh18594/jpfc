using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class seedidlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IdentificationDocument",
                columns: new[] { "IdentificationDocumentId", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("78faffc2-601a-4a06-b7ef-0e3b5a7f8b96"), "Driver's License", null },
                    { new Guid("4894bd7a-0476-4ea0-97c9-cd82fb40f2b2"), "Health Card", null },
                    { new Guid("4eed5d73-e2b9-406f-979f-7c124813eef3"), "Passport", null },
                    { new Guid("5bb91152-bee1-4afb-8c0b-aa95e6c4a9b5"), "PR Card", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentificationDocument",
                keyColumn: "IdentificationDocumentId",
                keyValue: new Guid("4894bd7a-0476-4ea0-97c9-cd82fb40f2b2"));

            migrationBuilder.DeleteData(
                table: "IdentificationDocument",
                keyColumn: "IdentificationDocumentId",
                keyValue: new Guid("4eed5d73-e2b9-406f-979f-7c124813eef3"));

            migrationBuilder.DeleteData(
                table: "IdentificationDocument",
                keyColumn: "IdentificationDocumentId",
                keyValue: new Guid("5bb91152-bee1-4afb-8c0b-aa95e6c4a9b5"));

            migrationBuilder.DeleteData(
                table: "IdentificationDocument",
                keyColumn: "IdentificationDocumentId",
                keyValue: new Guid("78faffc2-601a-4a06-b7ef-0e3b5a7f8b96"));
        }
    }
}
