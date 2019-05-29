using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class refactoredforsharedclients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(" DELETE FROM [ClientBelonging] ");
            migrationBuilder.Sql(" DELETE FROM [Client] ");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_IdentificationDocument_IdentificationDocumentId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientBelonging_Client_ClientId",
                table: "ClientBelonging");

            migrationBuilder.DropIndex(
                name: "IX_Client_IdentificationDocumentId",
                table: "Client");

            migrationBuilder.DeleteData(
                table: "IdentificationDocument",
                keyColumn: "IdentificationDocumentId",
                keyValue: new Guid("4894bd7a-0476-4ea0-97c9-cd82fb40f2b2"));

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ClientBelonging");

            migrationBuilder.DropColumn(
                name: "IdentificationDocumentId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "IdentificationDocumentNumber",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientBelonging",
                newName: "ClientReceiptId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientBelonging_ClientId",
                table: "ClientBelonging",
                newName: "IX_ClientBelonging_ClientReceiptId");

            migrationBuilder.CreateTable(
                name: "ClientIdentification",
                columns: table => new
                {
                    ClientIdentificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    IdentificationDocumentId = table.Column<Guid>(nullable: true),
                    IdentificationDocumentNumber = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedUserId = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    AuditUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdentification", x => x.ClientIdentificationId);
                    table.ForeignKey(
                        name: "FK_ClientIdentification_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientIdentification_IdentificationDocument_IdentificationDocumentId",
                        column: x => x.IdentificationDocumentId,
                        principalTable: "IdentificationDocument",
                        principalColumn: "IdentificationDocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientReceipt",
                columns: table => new
                {
                    ClientReceiptId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ReceiptNumber = table.Column<string>(maxLength: 255, nullable: true),
                    ClientIdentificationId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    AuditUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReceipt", x => x.ClientReceiptId);
                    table.ForeignKey(
                        name: "FK_ClientReceipt_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientReceipt_ClientIdentification_ClientIdentificationId",
                        column: x => x.ClientIdentificationId,
                        principalTable: "ClientIdentification",
                        principalColumn: "ClientIdentificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientIdentification_ClientId",
                table: "ClientIdentification",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientIdentification_IdentificationDocumentId",
                table: "ClientIdentification",
                column: "IdentificationDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReceipt_ClientId",
                table: "ClientReceipt",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReceipt_ClientIdentificationId",
                table: "ClientReceipt",
                column: "ClientIdentificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientBelonging_ClientReceipt_ClientReceiptId",
                table: "ClientBelonging",
                column: "ClientReceiptId",
                principalTable: "ClientReceipt",
                principalColumn: "ClientReceiptId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientBelonging_ClientReceipt_ClientReceiptId",
                table: "ClientBelonging");

            migrationBuilder.DropTable(
                name: "ClientReceipt");

            migrationBuilder.DropTable(
                name: "ClientIdentification");

            migrationBuilder.RenameColumn(
                name: "ClientReceiptId",
                table: "ClientBelonging",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientBelonging_ClientReceiptId",
                table: "ClientBelonging",
                newName: "IX_ClientBelonging_ClientId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ClientBelonging",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificationDocumentId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationDocumentNumber",
                table: "Client",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "IdentificationDocument",
                columns: new[] { "IdentificationDocumentId", "Name", "SortOrder" },
                values: new object[] { new Guid("4894bd7a-0476-4ea0-97c9-cd82fb40f2b2"), "Health Card", null });

            migrationBuilder.CreateIndex(
                name: "IX_Client_IdentificationDocumentId",
                table: "Client",
                column: "IdentificationDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_IdentificationDocument_IdentificationDocumentId",
                table: "Client",
                column: "IdentificationDocumentId",
                principalTable: "IdentificationDocument",
                principalColumn: "IdentificationDocumentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientBelonging_Client_ClientId",
                table: "ClientBelonging",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
