using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddAllowedTimeAndAlowedMemoryColumnsToProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AllowedMemoryInMegaBytes",
                table: "Problems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AllowedTimeInSeconds",
                table: "Problems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedMemoryInMegaBytes",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "AllowedTimeInSeconds",
                table: "Problems");
        }
    }
}
