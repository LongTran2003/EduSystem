using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5132d498-aa12-41a0-9569-571b4be1fbde", "AQAAAAIAAYagAAAAEOpoBHe1vho0jjTA9qQ9M4ndJYTHSGHM05YNamAnO1JqIzzw/egWj6jSI9falog8ow==", "d7d5986c-4b6e-4932-bdc1-e99dc36b7b91" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f7e355f-3a51-4092-9f87-f90c8437be05", "AQAAAAIAAYagAAAAEMRPEE9+qH0UXLAKqLyj5pzNVIgOxzpHVTXBTnCpBZCYIITu+lljgLrvaGEvKHQJww==", "602fc432-5377-458d-9162-796db1292887" });
        }
    }
}
