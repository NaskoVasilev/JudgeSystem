using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AllowedIpAddressEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowedIpAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedIpAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllowedIpAddressContests",
                columns: table => new
                {
                    AllowedIpAddressId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedIpAddressContests", x => new { x.AllowedIpAddressId, x.ContestId });
                    table.ForeignKey(
                        name: "FK_AllowedIpAddressContests_AllowedIpAddresses_AllowedIpAddressId",
                        column: x => x.AllowedIpAddressId,
                        principalTable: "AllowedIpAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllowedIpAddressContests_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedIpAddressContests_ContestId",
                table: "AllowedIpAddressContests",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedIpAddresses_IsDeleted",
                table: "AllowedIpAddresses",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedIpAddressContests");

            migrationBuilder.DropTable(
                name: "AllowedIpAddresses");
        }
    }
}
