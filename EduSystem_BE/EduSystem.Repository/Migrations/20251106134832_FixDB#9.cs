using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Matrices");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Matrices");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Matrices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f60686f-e63b-40af-84aa-4b1e1936ab31", "AQAAAAIAAYagAAAAEKJAsl/aoSNOSLKTs3KjmlpiDu6BgjyUL0Z5ZVuP1dhEqkAN/Jp97dctDYLWNpUsPA==", "0d1b8de9-8431-49ea-94a4-672b9d462d3a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Matrices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Matrices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Matrices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "601130b9-353a-4cb2-afd9-34dc32e0c66e", "AQAAAAIAAYagAAAAEOIQPomLCHoh6oODOMJYGHf+zS3rTHpItkW78vNIyufBIYwdzpmmXz4txVR/ET0jUw==", "b66cfb94-7e1f-4b3d-881f-34e3f463a741" });
        }
    }
}
