using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddColumnsToExecutedTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutedSuccessfully",
                table: "ExecutedTests");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "ExecutedTests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExecutionResultType",
                table: "ExecutedTests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "MemoryUsed",
                table: "ExecutedTests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "TimeUsed",
                table: "ExecutedTests",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "ExecutedTests");

            migrationBuilder.DropColumn(
                name: "ExecutionResultType",
                table: "ExecutedTests");

            migrationBuilder.DropColumn(
                name: "MemoryUsed",
                table: "ExecutedTests");

            migrationBuilder.DropColumn(
                name: "TimeUsed",
                table: "ExecutedTests");

            migrationBuilder.AddColumn<bool>(
                name: "ExecutedSuccessfully",
                table: "ExecutedTests",
                nullable: false,
                defaultValue: false);
        }
    }
}
