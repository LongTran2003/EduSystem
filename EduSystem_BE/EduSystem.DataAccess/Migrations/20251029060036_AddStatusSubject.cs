using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Subjects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Movok-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99a09ee9-ab5f-41c6-b47a-1160e9c39731", "AQAAAAIAAYagAAAAEG0LD3cDLVkebsvPoM8p8U0AP8NFkrhiDEsra75R8n5VT/K+KyyP7xIo3mYmP277Xw==", "fe497f28-75e3-4b11-a14e-2a0c6be33e65" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Teachers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Teachers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Teachers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Teachers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Subjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Movok-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ba91945-2fec-4925-93b7-5278e3320f8c", "AQAAAAIAAYagAAAAEKn8q5NbZ36Rx5ogoyE9rixXWviFlgVZ2Gfl/TBPlK9mJjbQYM+xalpV+Gg4W4k4nA==", "9e00f0ad-e565-4b33-b55c-a96bea59516b" });
        }
    }
}
