using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixDB10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "Students",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d10b7157-1050-4ff6-b065-c42a1d609713", "AQAAAAIAAYagAAAAEGieulUbZiioVRXBAgrhcnthkWbZ571Kkl55hM3cfKRUfrVs7mD9XgL7z097gFp4oA==", "bb3739e1-4443-4b17-97c3-91bce8a6d2d2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "Students",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f60686f-e63b-40af-84aa-4b1e1936ab31", "AQAAAAIAAYagAAAAEKJAsl/aoSNOSLKTs3KjmlpiDu6BgjyUL0Z5ZVuP1dhEqkAN/Jp97dctDYLWNpUsPA==", "0d1b8de9-8431-49ea-94a4-672b9d462d3a" });
        }
    }
}
