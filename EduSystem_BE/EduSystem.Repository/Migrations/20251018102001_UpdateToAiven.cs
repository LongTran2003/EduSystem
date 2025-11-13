using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToAiven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a5393a31-593d-4206-8e99-32a0e8e56789", "AQAAAAIAAYagAAAAEDyKQu8FvbixsAOrGnRpqwAujjXhFscRvMYYvV6OzSJWw4kcVAfdwDPWQKlfsRJl7w==", "4f84b734-8ffa-4417-ae22-865610000ab3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5e357e54-956a-44ff-9336-ec5a15d6e885", "AQAAAAIAAYagAAAAEOH4pu7GyFRnJeC8J/KEyUnYRyotZ+yD0j8jKDKXwBTc6EnT9D+/SWycbc2j07phHQ==", "14899d2c-5e4b-483e-a135-3f54450a2b36" });
        }
    }
}
