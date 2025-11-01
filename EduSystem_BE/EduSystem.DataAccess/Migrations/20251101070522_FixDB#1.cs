using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lessons_LessonId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Subjects_SubjectId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Subjects_SubjectId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_StudentQuizAttempts_StudentQuizAttemptId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Students_StudentId",
                table: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "StudentQuizAttempts");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses");

            migrationBuilder.DropIndex(
                name: "IX_StudentProgresses_StudentId",
                table: "StudentProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_StudentId",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_StudentQuizAttemptId",
                table: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_LessonId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SubjectId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "EnglishProficiency",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "OfficePhone",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentProgressId",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "StudentAnswerId",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "StudentQuizAttemptId",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "QuizQuestionId",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "GradeLevel",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "StudentProgresses",
                newName: "LastAccessDate");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "StudentAnswers",
                newName: "AttemptId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Quizzes",
                newName: "QuizName");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Quizzes",
                newName: "MatrixId");

            migrationBuilder.RenameColumn(
                name: "GradeLevel",
                table: "Quizzes",
                newName: "EnglishLevel");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_SubjectId",
                table: "Quizzes",
                newName: "IX_Quizzes_MatrixId");

            migrationBuilder.RenameColumn(
                name: "GradeLevel",
                table: "Questions",
                newName: "EnglishLevel");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Lessons",
                newName: "LessonName");

            migrationBuilder.RenameColumn(
                name: "LessonType",
                table: "Lessons",
                newName: "Skill");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "LessonContents",
                newName: "Description");

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Teachers",
                type: "numeric(3,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TimeSpent",
                table: "StudentProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletionStatus",
                table: "StudentProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "StudentProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "StudentAnswers",
                type: "numeric(5,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrect",
                table: "StudentAnswers",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "StudentAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherFeedback",
                table: "StudentAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Quizzes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quizzes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Quizzes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                table: "Quizzes",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Skill",
                table: "Quizzes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "Questions",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SkillType",
                table: "Questions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Lessons",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Lessons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "Lessons",
                type: "uuid",
                maxLength: 200,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "LessonContents",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceUrl",
                table: "LessonContents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Answers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Answers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "LessonId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                columns: new[] { "AttemptId", "QuestionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions",
                columns: new[] { "QuizId", "QuestionId" });

            migrationBuilder.CreateTable(
                name: "Matrices",
                columns: table => new
                {
                    MatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EnglishLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SkillFocus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matrices", x => x.MatrixId);
                    table.ForeignKey(
                        name: "FK_Matrices_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAttemptId);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EnglishLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LearningObjectives = table.Column<string>(type: "text", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatrixDetails",
                columns: table => new
                {
                    DetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    SkillType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    QuestionCount = table.Column<int>(type: "integer", nullable: false),
                    ScorePerQuestion = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrixDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_MatrixDetails_Matrices_MatrixId",
                        column: x => x.MatrixId,
                        principalTable: "Matrices",
                        principalColumn: "MatrixId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Movok-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37514641-73db-4c7c-859f-41061c3db431", "AQAAAAIAAYagAAAAEOY0YnYKQImxUsHbEu0a3xuF2lUT78CWyapPbSB5bFUuoxAbmhZShQX6oE/xx3JCrA==", "f8d4dac1-7ff2-481b-9161-3c839a200849" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_UnitId",
                table: "Lessons",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_TeacherId",
                table: "Matrices",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_MatrixDetails_MatrixId",
                table: "MatrixDetails",
                column: "MatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_TeacherId",
                table: "Units",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Units_UnitId",
                table: "Lessons",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Matrices_MatrixId",
                table: "Quizzes",
                column: "MatrixId",
                principalTable: "Matrices",
                principalColumn: "MatrixId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_QuizAttempts_AttemptId",
                table: "StudentAnswers",
                column: "AttemptId",
                principalTable: "QuizAttempts",
                principalColumn: "QuizAttemptId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Units_UnitId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Matrices_MatrixId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_QuizAttempts_AttemptId",
                table: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "MatrixDetails");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Matrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_UnitId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CompletionStatus",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Answers",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "TeacherFeedback",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Skill",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SkillType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "ResourceUrl",
                table: "LessonContents");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "LastAccessDate",
                table: "StudentProgresses",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "AttemptId",
                table: "StudentAnswers",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "QuizName",
                table: "Quizzes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "MatrixId",
                table: "Quizzes",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "EnglishLevel",
                table: "Quizzes",
                newName: "GradeLevel");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_MatrixId",
                table: "Quizzes",
                newName: "IX_Quizzes_SubjectId");

            migrationBuilder.RenameColumn(
                name: "EnglishLevel",
                table: "Questions",
                newName: "GradeLevel");

            migrationBuilder.RenameColumn(
                name: "Skill",
                table: "Lessons",
                newName: "LessonType");

            migrationBuilder.RenameColumn(
                name: "LessonName",
                table: "Lessons",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "LessonContents",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Teachers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "Teachers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Teachers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Teachers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishProficiency",
                table: "Teachers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Teachers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficePhone",
                table: "Teachers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Teachers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Students",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GraduationDate",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TimeSpent",
                table: "StudentProgresses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentProgressId",
                table: "StudentProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "StudentProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "StudentProgresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "StudentProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "StudentProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentProgresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "StudentAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrect",
                table: "StudentAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentAnswerId",
                table: "StudentAnswers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "StudentAnswers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentQuizAttemptId",
                table: "StudentAnswers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "StudentAnswers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeSpent",
                table: "StudentAnswers",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Quizzes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quizzes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Quizzes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Quizzes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Quizzes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Quizzes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizQuestionId",
                table: "QuizQuestions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "QuizQuestions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "QuizQuestions",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Questions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Questions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                table: "Questions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Lessons",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lessons",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Lessons",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradeLevel",
                table: "Lessons",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Lessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Lessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "LessonContents",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "LessonContents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileSize",
                table: "LessonContents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "LessonContents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "LessonContents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LessonContents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Answers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentProgresses",
                table: "StudentProgresses",
                column: "StudentProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                column: "StudentAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions",
                column: "QuizQuestionId");

            migrationBuilder.CreateTable(
                name: "StudentQuizAttempts",
                columns: table => new
                {
                    StudentQuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxScore = table.Column<int>(type: "integer", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TimeSpent = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizAttempts", x => x.StudentQuizAttemptId);
                    table.ForeignKey(
                        name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentQuizAttempts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    GradeLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    SubjectCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubjectName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Movok-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99a09ee9-ab5f-41c6-b47a-1160e9c39731", "AQAAAAIAAYagAAAAEG0LD3cDLVkebsvPoM8p8U0AP8NFkrhiDEsra75R8n5VT/K+KyyP7xIo3mYmP277Xw==", "fe497f28-75e3-4b11-a14e-2a0c6be33e65" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_StudentId",
                table: "StudentProgresses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_StudentId",
                table: "StudentAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_StudentQuizAttemptId",
                table: "StudentAnswers",
                column: "StudentQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LessonId",
                table: "Questions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubjectId",
                table: "Questions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAttempts_QuizId",
                table: "StudentQuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAttempts_StudentId",
                table: "StudentQuizAttempts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lessons_LessonId",
                table: "Questions",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Subjects_SubjectId",
                table: "Questions",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Subjects_SubjectId",
                table: "Quizzes",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_StudentQuizAttempts_StudentQuizAttemptId",
                table: "StudentAnswers",
                column: "StudentQuizAttemptId",
                principalTable: "StudentQuizAttempts",
                principalColumn: "StudentQuizAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Students_StudentId",
                table: "StudentAnswers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
