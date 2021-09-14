using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class OrderableEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "Resources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "Problems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "Lessons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderBy",
                table: "Courses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Courses");
        }
    }
}
