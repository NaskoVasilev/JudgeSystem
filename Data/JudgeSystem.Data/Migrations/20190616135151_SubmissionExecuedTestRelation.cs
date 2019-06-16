using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class SubmissionExecuedTestRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Submission",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Submission",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmissionId",
                table: "ExecutedTests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutedTests_SubmissionId",
                table: "ExecutedTests",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutedTests_Submission_SubmissionId",
                table: "ExecutedTests",
                column: "SubmissionId",
                principalTable: "Submission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutedTests_Submission_SubmissionId",
                table: "ExecutedTests");

            migrationBuilder.DropIndex(
                name: "IX_ExecutedTests_SubmissionId",
                table: "ExecutedTests");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "SubmissionId",
                table: "ExecutedTests");
        }
    }
}
