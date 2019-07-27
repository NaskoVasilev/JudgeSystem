using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JudgeSystem.Data.Migrations
{
    public partial class AddPracticeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PracticeId",
                table: "Submissions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Practices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    LessonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practices_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPractices",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PracticeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPractices", x => new { x.PracticeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserPractices_Practices_PracticeId",
                        column: x => x.PracticeId,
                        principalTable: "Practices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPractices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PracticeId",
                table: "Submissions",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_IsDeleted",
                table: "Practices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_LessonId",
                table: "Practices",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPractices_UserId",
                table: "UserPractices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Practices_PracticeId",
                table: "Submissions",
                column: "PracticeId",
                principalTable: "Practices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Practices_PracticeId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "UserPractices");

            migrationBuilder.DropTable(
                name: "Practices");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_PracticeId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Submissions");
        }
    }
}
