using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class FixProblemColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowedTimeInMiliseconds",
                table: "Problems",
                newName: "AllowedTimeInMilliseconds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowedTimeInMilliseconds",
                table: "Problems",
                newName: "AllowedTimeInMiliseconds");
        }
    }
}
