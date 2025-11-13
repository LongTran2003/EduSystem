using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixDB6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f7e355f-3a51-4092-9f87-f90c8437be05", "AQAAAAIAAYagAAAAEMRPEE9+qH0UXLAKqLyj5pzNVIgOxzpHVTXBTnCpBZCYIITu+lljgLrvaGEvKHQJww==", "602fc432-5377-458d-9162-796db1292887" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c3acbd47-0cad-434b-a41e-878606dfb9a3", "AQAAAAIAAYagAAAAENN0T4LGWAIv2y9BDnhljoqGiHnqntC+Jqss6LFpmc1r/ocMYl4DYwCVelejxpK9cg==", "87d4be2c-5eb0-440a-9e10-1f8d6ac80f94" });
        }
    }
}
