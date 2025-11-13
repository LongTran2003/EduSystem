using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixDB11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Students_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Teachers_TeacherId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgresses_Lessons_LessonId",
                table: "StudentProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgresses_Students_StudentId",
                table: "StudentProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "TimeSpent",
                table: "StudentProgresses",
                newName: "TotalTimeSpent");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "StudentProgresses",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CompletionStatus",
                table: "StudentProgresses",
                newName: "TotalLessons");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Teachers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "School",
                table: "Students",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Students",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolLevel",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessDate",
                table: "StudentProgresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "StudentProgresses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ProgressId",
                table: "StudentProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "AverageScore",
                table: "StudentProgresses",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CompletedLessons",
                table: "StudentProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StudentProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "StudentProgresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProgressPercentage",
                table: "StudentProgresses",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "StudentProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "StudentProgresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Quizzes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Quizzes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "QuizQuestions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizQuestionId",
                table: "QuizQuestions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "QuizQuestions",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "AIPrompt",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyLevel",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAIGenerated",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AIPrompt",
                table: "LessonContents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAIGenerated",
                table: "LessonContents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "LessonContents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses",
                column: "ProgressId");

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SchoolYear = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SchoolLevel = table.Column<int>(type: "integer", nullable: false),
                    MaxStudents = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    StudentClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrolledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.StudentClassId);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15a2ad5c-8c37-48fb-a7ad-6e1b99fd1e43", "AQAAAAIAAYagAAAAEFfCEfdTFN8Nv19YRvrUhs6TLn3hF8xjTtHkSNKcEYCAybD3A6J1UnNU2f7HC4pLmg==", "747accf1-4ed7-4c89-9192-6542fc59917c" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_StudentId_UnitId",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "UnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_UnitId",
                table: "StudentProgresses",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ClassId",
                table: "Quizzes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassName",
                table: "Classes",
                column: "ClassName");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_ClassId",
                table: "StudentClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId_ClassId",
                table: "StudentClasses",
                columns: new[] { "StudentId", "ClassId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Students_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Teachers_TeacherId",
                table: "Quizzes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgresses_Lessons_LessonId",
                table: "StudentProgresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgresses_Students_StudentId",
                table: "StudentProgresses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgresses_Units_UnitId",
                table: "StudentProgresses",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Students_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Teachers_TeacherId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgresses_Lessons_LessonId",
                table: "StudentProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgresses_Students_StudentId",
                table: "StudentProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgresses_Units_UnitId",
                table: "StudentProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentProgresses_StudentId_UnitId",
                table: "StudentProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentProgresses_UnitId",
                table: "StudentProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_ClassId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolLevel",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "AverageScore",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "CompletedLessons",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "ProgressPercentage",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "QuizQuestionId",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "AIPrompt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsAIGenerated",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AIPrompt",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "IsAIGenerated",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "LessonContents");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "StudentProgresses",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "TotalTimeSpent",
                table: "StudentProgresses",
                newName: "TimeSpent");

            migrationBuilder.RenameColumn(
                name: "TotalLessons",
                table: "StudentProgresses",
                newName: "CompletionStatus");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Teachers",
                type: "numeric(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Teachers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "School",
                table: "Students",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Students",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "StudentProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessDate",
                table: "StudentProgresses",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "LessonId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d10b7157-1050-4ff6-b065-c42a1d609713", "AQAAAAIAAYagAAAAEGieulUbZiioVRXBAgrhcnthkWbZ571Kkl55hM3cfKRUfrVs7mD9XgL7z097gFp4oA==", "bb3739e1-4443-4b17-97c3-91bce8a6d2d2" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Students_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Teachers_TeacherId",
                table: "Quizzes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgresses_Lessons_LessonId",
                table: "StudentProgresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgresses_Students_StudentId",
                table: "StudentProgresses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
