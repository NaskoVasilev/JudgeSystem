using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class RenameAllowedTimeInSecondsToRenameAllowedTimeInMiliseconds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowedTimeInSeconds",
                table: "Problems",
                newName: "AllowedTimeInMiliseconds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowedTimeInMiliseconds",
                table: "Problems",
                newName: "AllowedTimeInSeconds");
        }
    }
}
