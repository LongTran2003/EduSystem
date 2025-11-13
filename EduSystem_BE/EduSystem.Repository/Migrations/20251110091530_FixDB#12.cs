using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Classes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Classes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9991b27-b511-47c3-9177-1f0de393249a", "AQAAAAIAAYagAAAAEJQlXwFde/ZbRep6wh70YnJ8wjgRo6umTY3d6E+EOjbMpC4PkulA9B9qKd127D9Y+w==", "04dba7cb-a5b9-4176-be30-efe30b24a100" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Classes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15a2ad5c-8c37-48fb-a7ad-6e1b99fd1e43", "AQAAAAIAAYagAAAAEFfCEfdTFN8Nv19YRvrUhs6TLn3hF8xjTtHkSNKcEYCAybD3A6J1UnNU2f7HC4pLmg==", "747accf1-4ed7-4c89-9192-6542fc59917c" });
        }
    }
}
