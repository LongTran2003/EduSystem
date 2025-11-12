using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixDB19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentAnswerId",
                table: "StudentAnswers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                column: "StudentAnswerId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d2bbd09-76d8-4c25-b457-a5d920833e2e", "AQAAAAIAAYagAAAAELWm5ntpk8Ig0s9i9nU5EmgCTqDkFvC2nhFyrTjk+Qlwuf6BLmrzvRw4Dgop7xvyQQ==", "40db138a-c839-479a-8797-5589d3a911e4" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_AttemptId_QuestionId",
                table: "StudentAnswers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_AttemptId_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "StudentAnswerId",
                table: "StudentAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                columns: new[] { "AttemptId", "QuestionId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2c86037-ad54-41a6-8e13-c4bc1363342d", "AQAAAAIAAYagAAAAEBxLbPvlgbmCQfKjftMOD0mSWyLs3C6+nn1uWcwtnQoVtItVt/epyc3lN9ZdDm1gKw==", "e554e44d-5540-42fb-a835-1987f98df22a" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
