using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jpfc.Data.Migrations
{
    public partial class seedjobtypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobType",
                columns: new[] { "JobTypeId", "Type" },
                values: new object[,]
                {
                    { new Guid("524da1c1-41d1-48a3-ae86-9ed0b5697083"), "Full-time Permanent" },
                    { new Guid("2c848d1c-5384-44f8-a0ce-1b4b32262440"), "Part-time Permanent" },
                    { new Guid("6cfb1f29-ee23-41e0-89ed-082665a4bdfe"), "Full-time Temporary" },
                    { new Guid("92c30141-3d65-4131-805a-5d4d56dc1cf0"), "Part-time Temporary" },
                    { new Guid("2e9526db-e626-421e-8baf-88da023cc13b"), "Full-time Contract" },
                    { new Guid("e94a32b3-caf2-4824-8608-b13582618f5b"), "Part-time Contract" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("2c848d1c-5384-44f8-a0ce-1b4b32262440"));

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("2e9526db-e626-421e-8baf-88da023cc13b"));

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("524da1c1-41d1-48a3-ae86-9ed0b5697083"));

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("6cfb1f29-ee23-41e0-89ed-082665a4bdfe"));

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("92c30141-3d65-4131-805a-5d4d56dc1cf0"));

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: new Guid("e94a32b3-caf2-4824-8608-b13582618f5b"));
        }
    }
}
