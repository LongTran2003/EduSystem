using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "Movok-Admin" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Movok-Admin");

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"", ""ConcurrencyStamp"")
                VALUES ('8fa7c7bb-daa5-a660-bf02-82301a5eb32a', 'ADMIN', 'ADMIN', 'ADMIN')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Gender", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiry", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[] { "EduSystem-Admin", 0, "123 Admin St", new DateTime(2001, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "46e63db6-49cb-4915-8980-2c2323194381", "admin@gmail.com", true, "Admin", null, "https://example.com/avatar.png", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", null, null, "AQAAAAIAAYagAAAAED2fTUg5h3Z/Z5jxLTTIUqBYJvQes91aNOi91puYpN+pZunKbQaC5H1Vp2mptwce0Q==", "1234567890", true, "fa5c7679-c050-4396-a7a1-b7226a03acf2", "Active", false, "admin@gmail.com" });

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetUserRoles"" (""UserId"", ""RoleId"")
                VALUES ('EduSystem-Admin', '8fa7c7bb-daa5-a660-bf02-82301a5eb32a')
                ON CONFLICT (""UserId"", ""RoleId"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "EduSystem-Admin" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Gender", "ImageUrl", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpCode", "OtpExpiry", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[] { "Movok-Admin", 0, "123 Admin St", new DateTime(2001, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), "37514641-73db-4c7c-859f-41061c3db431", "admin@gmail.com", true, "Admin", null, "https://example.com/avatar.png", true, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", null, null, "AQAAAAIAAYagAAAAEOY0YnYKQImxUsHbEu0a3xuF2lUT78CWyapPbSB5bFUuoxAbmhZShQX6oE/xx3JCrA==", "1234567890", true, "f8d4dac1-7ff2-481b-9161-3c839a200849", "Active", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8fa7c7bb-daa5-a660-bf02-82301a5eb32a", "Movok-Admin" });
        }
    }
}
