using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddPropertyTestingStrategyToProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestingStrategy",
                table: "Problems",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestingStrategy",
                table: "Problems");
        }
    }
}
