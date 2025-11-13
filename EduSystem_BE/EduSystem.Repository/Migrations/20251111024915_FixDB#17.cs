using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixDB17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionOrder",
                table: "QuizQuestions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d61e59e-8b5f-41dc-ab10-93f0138b342a", "AQAAAAIAAYagAAAAENsJrkHoBXgrcbHOJzxbp9rDBhJEH0YKc5+WJvnWo8oWgl3HEBUVmn4y6IOYgDbX0A==", "43c2a2c8-b8d4-4ec2-aba1-3328bd4ea472" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "QuizQuestions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20f6a7f8-b32d-4b0f-ac4d-85a13fdfd99f", "AQAAAAIAAYagAAAAEC1nTk09l5/2VXe2rk4jBtcrYaZB6TvUSx4rPNVlQIyzTON+IukU2eICO+ncHtZvNw==", "15df3a25-a0cf-42cd-a4d1-7dc571573a24" });
        }
    }
}
