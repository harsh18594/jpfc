using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class seedmetalandkarat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Price_Weight_WeightId",
                table: "Price");

            migrationBuilder.DropTable(
                name: "Weight");

            migrationBuilder.DropIndex(
                name: "IX_Price_WeightId",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "WeightId",
                table: "Price");

            migrationBuilder.AddColumn<Guid>(
                name: "KaratId",
                table: "Price",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Karat",
                columns: table => new
                {
                    KaratId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    InActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karat", x => x.KaratId);
                });

            migrationBuilder.InsertData(
                table: "Karat",
                columns: new[] { "KaratId", "InActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0bab0d22-f831-4b6c-b177-c623ba4bf5b9"), false, "10K" },
                    { new Guid("775d10c4-a955-4039-aaf6-16a80b0759f7"), false, "14K" },
                    { new Guid("4aaf69a9-7bb0-4f70-956d-4f55cd98fe1e"), false, "18K" },
                    { new Guid("4da2d061-e089-4c8d-bfa4-534a301e0c87"), false, "22K" },
                    { new Guid("d9fb756f-933d-4cfa-9dc3-76714b84b256"), false, "24K" }
                });

            migrationBuilder.InsertData(
                table: "Metal",
                columns: new[] { "MetalId", "InActive", "Name" },
                values: new object[,]
                {
                    { new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), false, "Gold" },
                    { new Guid("2a883efe-13fa-4a50-ad2b-ae49b034c8b0"), false, "Silver" },
                    { new Guid("b0fd2523-63a0-4cf3-940c-627617b3196f"), false, "Platinum" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_KaratId",
                table: "Price",
                column: "KaratId");

            migrationBuilder.AddForeignKey(
                name: "FK_Price_Karat_KaratId",
                table: "Price",
                column: "KaratId",
                principalTable: "Karat",
                principalColumn: "KaratId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Price_Karat_KaratId",
                table: "Price");

            migrationBuilder.DropTable(
                name: "Karat");

            migrationBuilder.DropIndex(
                name: "IX_Price_KaratId",
                table: "Price");

            migrationBuilder.DeleteData(
                table: "Metal",
                keyColumn: "MetalId",
                keyValue: new Guid("2a883efe-13fa-4a50-ad2b-ae49b034c8b0"));

            migrationBuilder.DeleteData(
                table: "Metal",
                keyColumn: "MetalId",
                keyValue: new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"));

            migrationBuilder.DeleteData(
                table: "Metal",
                keyColumn: "MetalId",
                keyValue: new Guid("b0fd2523-63a0-4cf3-940c-627617b3196f"));

            migrationBuilder.DropColumn(
                name: "KaratId",
                table: "Price");

            migrationBuilder.AddColumn<Guid>(
                name: "WeightId",
                table: "Price",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    WeightId = table.Column<Guid>(nullable: false),
                    InActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.WeightId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_WeightId",
                table: "Price",
                column: "WeightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Price_Weight_WeightId",
                table: "Price",
                column: "WeightId",
                principalTable: "Weight",
                principalColumn: "WeightId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
