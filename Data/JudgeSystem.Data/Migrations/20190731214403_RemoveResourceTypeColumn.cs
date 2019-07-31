using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class RemoveResourceTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Resources",
                newName: "FilePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Resources",
                newName: "Link");

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "Resources",
                nullable: false,
                defaultValue: 0);
        }
    }
}
