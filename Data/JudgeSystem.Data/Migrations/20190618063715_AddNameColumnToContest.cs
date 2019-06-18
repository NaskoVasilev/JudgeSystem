using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddNameColumnToContest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contest_Lessons_LessonId",
                table: "Contest");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contest_ContestId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContest_Contest_ContestId",
                table: "UserContest");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContest_AspNetUsers_UserId",
                table: "UserContest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContest",
                table: "UserContest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contest",
                table: "Contest");

            migrationBuilder.RenameTable(
                name: "UserContest",
                newName: "UserContests");

            migrationBuilder.RenameTable(
                name: "Contest",
                newName: "Contests");

            migrationBuilder.RenameIndex(
                name: "IX_UserContest_ContestId",
                table: "UserContests",
                newName: "IX_UserContests_ContestId");

            migrationBuilder.RenameIndex(
                name: "IX_Contest_LessonId",
                table: "Contests",
                newName: "IX_Contests_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Contest_IsDeleted",
                table: "Contests",
                newName: "IX_Contests_IsDeleted");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contests",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContests",
                table: "UserContests",
                columns: new[] { "UserId", "ContestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contests",
                table: "Contests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Lessons_LessonId",
                table: "Contests",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Contests_ContestId",
                table: "Submission",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContests_Contests_ContestId",
                table: "UserContests",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContests_AspNetUsers_UserId",
                table: "UserContests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Lessons_LessonId",
                table: "Contests");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contests_ContestId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContests_Contests_ContestId",
                table: "UserContests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContests_AspNetUsers_UserId",
                table: "UserContests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContests",
                table: "UserContests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contests",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contests");

            migrationBuilder.RenameTable(
                name: "UserContests",
                newName: "UserContest");

            migrationBuilder.RenameTable(
                name: "Contests",
                newName: "Contest");

            migrationBuilder.RenameIndex(
                name: "IX_UserContests_ContestId",
                table: "UserContest",
                newName: "IX_UserContest_ContestId");

            migrationBuilder.RenameIndex(
                name: "IX_Contests_LessonId",
                table: "Contest",
                newName: "IX_Contest_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Contests_IsDeleted",
                table: "Contest",
                newName: "IX_Contest_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContest",
                table: "UserContest",
                columns: new[] { "UserId", "ContestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contest",
                table: "Contest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contest_Lessons_LessonId",
                table: "Contest",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Contest_ContestId",
                table: "Submission",
                column: "ContestId",
                principalTable: "Contest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContest_Contest_ContestId",
                table: "UserContest",
                column: "ContestId",
                principalTable: "Contest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContest_AspNetUsers_UserId",
                table: "UserContest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
