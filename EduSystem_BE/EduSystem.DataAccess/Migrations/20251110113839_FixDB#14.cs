using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Sửa Students table - Xóa School, thêm Class
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Students' AND column_name = 'School'
                    ) THEN
                        ALTER TABLE ""Students"" DROP COLUMN ""School"";
                    END IF;
                END $$;
            ");

            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Students' AND column_name = 'Class'
                    ) THEN
                        ALTER TABLE ""Students"" ADD COLUMN ""Class"" character varying(10) NULL;
                    END IF;
                END $$;
            ");

            // Xử lý StudentProgresses - Kiểm tra bảng có tồn tại không
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    -- Nếu bảng tồn tại, xóa nó
                    IF EXISTS (
                        SELECT 1 FROM information_schema.tables 
                        WHERE table_name = 'StudentProgresses'
                    ) THEN
                        DROP TABLE ""StudentProgresses"" CASCADE;
                    END IF;
                END $$;
            ");

            // Tạo bảng StudentProgresses mới
            migrationBuilder.CreateTable(
                name: "StudentProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedLessons = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalLessons = table.Column<int>(type: "integer", nullable: false),
                    AverageScore = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m),
                    LastAccessDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalTimeSpent = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_StudentProgresses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentProgresses_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_StudentId_UnitId",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "UnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_UnitId",
                table: "StudentProgresses",
                column: "UnitId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da7299ea-0c57-4c64-a213-656c6498662a", "AQAAAAIAAYagAAAAEBawvA9GYmCm9FWlZXTpP+1LnbJSbYxtpEaZEAVTUEX72oWpwIk/BkU2qQu0eyZCmQ==", "3200a1d5-bb16-4383-9e98-413ae99f4dd5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentProgresses");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "School",
                table: "Students",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            // Tạo lại bảng StudentProgresses cũ nếu cần rollback
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""StudentProgresses"" (
                    ""StudentId"" uuid NOT NULL,
                    ""LessonId"" uuid NOT NULL,
                    ""CompletionStatus"" integer NOT NULL,
                    ""TimeSpent"" integer NOT NULL,
                    ""LastAccessDate"" timestamp with time zone NULL,
                    ""Notes"" text NULL,
                    ""Status"" text NOT NULL,
                    ""CreatedBy"" text NOT NULL,
                    ""CreatedTime"" timestamp with time zone NOT NULL,
                    ""UpdatedBy"" text NULL,
                    ""UpdatedTime"" timestamp with time zone NULL,
                    CONSTRAINT ""PK_StudentProgresses"" PRIMARY KEY (""StudentId"", ""LessonId""),
                    CONSTRAINT ""FK_StudentProgresses_Students_StudentId"" 
                        FOREIGN KEY (""StudentId"") REFERENCES ""Students"" (""StudentId"") ON DELETE CASCADE,
                    CONSTRAINT ""FK_StudentProgresses_Lessons_LessonId"" 
                        FOREIGN KEY (""LessonId"") REFERENCES ""Lessons"" (""LessonId"") ON DELETE CASCADE
                );
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78ad7190-d0a6-4896-a248-a5b3aeddf778", "AQAAAAIAAYagAAAAEJa9sl3bpea1IjBWZ6AG/5QOSFtztNnjISe4b3Do0KcegiYExryH7t4L5zv3TEgSDw==", "4770979e-1895-4a67-b9c9-a5cb0068c66f" });
        }
    }
}