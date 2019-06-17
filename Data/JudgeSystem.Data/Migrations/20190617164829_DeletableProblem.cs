using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class DeletableProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Problems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Problems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Problems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Problems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_IsDeleted",
                table: "Problems",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Problems_IsDeleted",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Problems");
        }
    }
}
