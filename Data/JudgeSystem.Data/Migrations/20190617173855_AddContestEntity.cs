using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddContestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContestId",
                table: "Submission",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    LessonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contest_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserContest",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ContestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContest", x => new { x.UserId, x.ContestId });
                    table.ForeignKey(
                        name: "FK_UserContest_Contest_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserContest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submission_ContestId",
                table: "Submission",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_Contest_IsDeleted",
                table: "Contest",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Contest_LessonId",
                table: "Contest",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContest_ContestId",
                table: "UserContest",
                column: "ContestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Contest_ContestId",
                table: "Submission",
                column: "ContestId",
                principalTable: "Contest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contest_ContestId",
                table: "Submission");

            migrationBuilder.DropTable(
                name: "UserContest");

            migrationBuilder.DropTable(
                name: "Contest");

            migrationBuilder.DropIndex(
                name: "IX_Submission_ContestId",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "ContestId",
                table: "Submission");
        }
    }
}
