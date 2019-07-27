using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class ChangeRelationBetweenPracticeAndLesson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Practices_LessonId",
                table: "Practices");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_LessonId",
                table: "Practices",
                column: "LessonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Practices_LessonId",
                table: "Practices");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_LessonId",
                table: "Practices",
                column: "LessonId");
        }
    }
}
