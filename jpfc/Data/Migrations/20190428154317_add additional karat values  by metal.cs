using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addadditionalkaratvaluesbymetal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MetalId",
                table: "Karat",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("0bab0d22-f831-4b6c-b177-c623ba4bf5b9"),
                column: "MetalId",
                value: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.UpdateData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("4aaf69a9-7bb0-4f70-956d-4f55cd98fe1e"),
                column: "MetalId",
                value: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.UpdateData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("4da2d061-e089-4c8d-bfa4-534a301e0c87"),
                column: "MetalId",
                value: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.UpdateData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("775d10c4-a955-4039-aaf6-16a80b0759f7"),
                column: "MetalId",
                value: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.UpdateData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("d9fb756f-933d-4cfa-9dc3-76714b84b256"),
                column: "MetalId",
                value: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.InsertData(
                table: "Karat",
                columns: new[] { "KaratId", "InActive", "MetalId", "Name" },
                values: new object[,]
                {
                    { new Guid("f47e14cb-9fc9-4aaf-9f2d-a64fc6a15c2f"), false, new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), "9K" },
                    { new Guid("06ff5e11-00cb-4cab-ae61-8237a13ae60f"), false, new Guid("b0fd2523-63a0-4cf3-940c-627617b3196f"), "950" },
                    { new Guid("ef76a7fe-0d8d-4814-83df-47a4a035d703"), false, new Guid("2a883efe-13fa-4a50-ad2b-ae49b034c8b0"), "925" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Karat_MetalId",
                table: "Karat",
                column: "MetalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Karat_Metal_MetalId",
                table: "Karat",
                column: "MetalId",
                principalTable: "Metal",
                principalColumn: "MetalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Karat_Metal_MetalId",
                table: "Karat");

            migrationBuilder.DropIndex(
                name: "IX_Karat_MetalId",
                table: "Karat");

            migrationBuilder.DeleteData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("06ff5e11-00cb-4cab-ae61-8237a13ae60f"));

            migrationBuilder.DeleteData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("ef76a7fe-0d8d-4814-83df-47a4a035d703"));

            migrationBuilder.DeleteData(
                table: "Karat",
                keyColumn: "KaratId",
                keyValue: new Guid("f47e14cb-9fc9-4aaf-9f2d-a64fc6a15c2f"));

            migrationBuilder.DropColumn(
                name: "MetalId",
                table: "Karat");
        }
    }
}
