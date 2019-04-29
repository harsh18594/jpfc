using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class addingtableforclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentificationDocument",
                columns: table => new
                {
                    IdentificationDocumentId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationDocument", x => x.IdentificationDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IdentificationDocumentId = table.Column<Guid>(nullable: true),
                    IdentificationDocumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ContactNumber = table.Column<string>(maxLength: 20, nullable: true),
                    CreatedUserId = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    AuditUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Client_IdentificationDocument_IdentificationDocumentId",
                        column: x => x.IdentificationDocumentId,
                        principalTable: "IdentificationDocument",
                        principalColumn: "IdentificationDocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientBelonging",
                columns: table => new
                {
                    ClientBelongingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    MetalId = table.Column<Guid>(nullable: true),
                    MetalOther = table.Column<string>(nullable: true),
                    KaratId = table.Column<Guid>(nullable: true),
                    KaratOther = table.Column<string>(nullable: true),
                    ItemWeight = table.Column<decimal>(nullable: true),
                    FinalPrice = table.Column<decimal>(nullable: true),
                    CreatedUserId = table.Column<string>(nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    AuditUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBelonging", x => x.ClientBelongingId);
                    table.ForeignKey(
                        name: "FK_ClientBelonging_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientBelonging_Karat_KaratId",
                        column: x => x.KaratId,
                        principalTable: "Karat",
                        principalColumn: "KaratId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientBelonging_Metal_MetalId",
                        column: x => x.MetalId,
                        principalTable: "Metal",
                        principalColumn: "MetalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_IdentificationDocumentId",
                table: "Client",
                column: "IdentificationDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBelonging_ClientId",
                table: "ClientBelonging",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBelonging_KaratId",
                table: "ClientBelonging",
                column: "KaratId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBelonging_MetalId",
                table: "ClientBelonging",
                column: "MetalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBelonging");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "IdentificationDocument");
        }
    }
}
