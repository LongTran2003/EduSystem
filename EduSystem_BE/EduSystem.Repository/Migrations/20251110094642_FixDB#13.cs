using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78ad7190-d0a6-4896-a248-a5b3aeddf778", "AQAAAAIAAYagAAAAEJa9sl3bpea1IjBWZ6AG/5QOSFtztNnjISe4b3Do0KcegiYExryH7t4L5zv3TEgSDw==", "4770979e-1895-4a67-b9c9-a5cb0068c66f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d10b7157-1050-4ff6-b065-c42a1d609713", "AQAAAAIAAYagAAAAEGieulUbZiioVRXBAgrhcnthkWbZ571Kkl55hM3cfKRUfrVs7mD9XgL7z097gFp4oA==", "bb3739e1-4443-4b17-97c3-91bce8a6d2d2" });
        }
    }
}
