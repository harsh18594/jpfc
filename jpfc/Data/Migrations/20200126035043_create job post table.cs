using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class createjobposttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobType",
                columns: table => new
                {
                    JobTypeId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobType", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "JobPost",
                columns: table => new
                {
                    JobPostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobTitle = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Requirements = table.Column<string>(nullable: true),
                    JobTypeId = table.Column<Guid>(nullable: true),
                    JobStartDate = table.Column<DateTime>(nullable: true),
                    Length = table.Column<string>(maxLength: 500, nullable: true),
                    Pay = table.Column<string>(maxLength: 500, nullable: true),
                    JobLocation = table.Column<string>(maxLength: 500, nullable: true),
                    JobCloseUtc = table.Column<DateTime>(nullable: true),
                    IsDraft = table.Column<bool>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<string>(nullable: true),
                    AuditUserId = table.Column<string>(nullable: true),
                    AuditUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPost", x => x.JobPostId);
                    table.ForeignKey(
                        name: "FK_JobPost_JobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobType",
                        principalColumn: "JobTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPost_JobTypeId",
                table: "JobPost",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobPost");

            migrationBuilder.DropTable(
                name: "JobType");
        }
    }
}
