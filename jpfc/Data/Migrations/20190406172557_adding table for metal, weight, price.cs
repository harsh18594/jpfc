using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addingtableformetalweightprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metal",
                columns: table => new
                {
                    MetalId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    InActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metal", x => x.MetalId);
                });

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    WeightId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    InActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.WeightId);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    PriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    MetalId = table.Column<Guid>(nullable: false),
                    WeightId = table.Column<Guid>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true),
                    AuditUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Price_Metal_MetalId",
                        column: x => x.MetalId,
                        principalTable: "Metal",
                        principalColumn: "MetalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Price_Weight_WeightId",
                        column: x => x.WeightId,
                        principalTable: "Weight",
                        principalColumn: "WeightId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_MetalId",
                table: "Price",
                column: "MetalId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_WeightId",
                table: "Price",
                column: "WeightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "Metal");

            migrationBuilder.DropTable(
                name: "Weight");
        }
    }
}
