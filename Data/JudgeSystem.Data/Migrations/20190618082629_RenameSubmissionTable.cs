using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class RenameSubmissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutedTests_Submission_SubmissionId",
                table: "ExecutedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contests_ContestId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Problems_ProblemId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_AspNetUsers_UserId",
                table: "Submission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submission",
                table: "Submission");

            migrationBuilder.RenameTable(
                name: "Submission",
                newName: "Submissions");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_UserId",
                table: "Submissions",
                newName: "IX_Submissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_ProblemId",
                table: "Submissions",
                newName: "IX_Submissions_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_ContestId",
                table: "Submissions",
                newName: "IX_Submissions_ContestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutedTests_Submissions_SubmissionId",
                table: "ExecutedTests",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Contests_ContestId",
                table: "Submissions",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                table: "Submissions",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutedTests_Submissions_SubmissionId",
                table: "ExecutedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Contests_ContestId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.RenameTable(
                name: "Submissions",
                newName: "Submission");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_UserId",
                table: "Submission",
                newName: "IX_Submission_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_ProblemId",
                table: "Submission",
                newName: "IX_Submission_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_ContestId",
                table: "Submission",
                newName: "IX_Submission_ContestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submission",
                table: "Submission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutedTests_Submission_SubmissionId",
                table: "ExecutedTests",
                column: "SubmissionId",
                principalTable: "Submission",
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
                name: "FK_Submission_Problems_ProblemId",
                table: "Submission",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_AspNetUsers_UserId",
                table: "Submission",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
