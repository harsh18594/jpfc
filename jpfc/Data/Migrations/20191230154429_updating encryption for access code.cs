using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class updatingencryptionforaccesscode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlainTextCode",
                table: "AccessCode",
                newName: "UniqueKey");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuditUtc",
                table: "AccessCode",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncryptedValue",
                table: "AccessCode",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditUtc",
                table: "AccessCode");

            migrationBuilder.DropColumn(
                name: "EncryptedValue",
                table: "AccessCode");

            migrationBuilder.RenameColumn(
                name: "UniqueKey",
                table: "AccessCode",
                newName: "PlainTextCode");
        }
    }
}
