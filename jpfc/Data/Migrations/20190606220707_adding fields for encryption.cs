using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addingfieldsforencryption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificationDocumentNumber",
                table: "ClientIdentification");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "IdentificaitonDocumentNumberUniqueKey",
                table: "ClientIdentification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationDocumentNumberEncrypted",
                table: "ClientIdentification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressEncrypted",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressUniqueKey",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumberEncrypted",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumberUniqueKey",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificaitonDocumentNumberUniqueKey",
                table: "ClientIdentification");

            migrationBuilder.DropColumn(
                name: "IdentificationDocumentNumberEncrypted",
                table: "ClientIdentification");

            migrationBuilder.DropColumn(
                name: "AddressEncrypted",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AddressUniqueKey",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ContactNumberEncrypted",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ContactNumberUniqueKey",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationDocumentNumber",
                table: "ClientIdentification",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Client",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Client",
                maxLength: 20,
                nullable: true);
        }
    }
}
