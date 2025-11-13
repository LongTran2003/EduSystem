using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToAiven1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d4c47ac-1da9-4e42-bb79-31c44c1115ad", "AQAAAAIAAYagAAAAEDzXhqCd4WmDibkui+3gog28rEywYKREP+49eOJx53IR+gqQ7shMC5NPEHv9pVOvpg==", "af990f85-3753-456d-bcd1-81407bed9a00" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a5393a31-593d-4206-8e99-32a0e8e56789", "AQAAAAIAAYagAAAAEDyKQu8FvbixsAOrGnRpqwAujjXhFscRvMYYvV6OzSJWw4kcVAfdwDPWQKlfsRJl7w==", "4f84b734-8ffa-4417-ae22-865610000ab3" });
        }
    }
}
