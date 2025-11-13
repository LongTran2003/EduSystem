using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c3acbd47-0cad-434b-a41e-878606dfb9a3", "AQAAAAIAAYagAAAAENN0T4LGWAIv2y9BDnhljoqGiHnqntC+Jqss6LFpmc1r/ocMYl4DYwCVelejxpK9cg==", "87d4be2c-5eb0-440a-9e10-1f8d6ac80f94" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c41abdb-d386-42ad-a754-390b844c18a9", "AQAAAAIAAYagAAAAEFUbBYlFc8kSGysRz+FeqLr50u+tCBnSFFXjSvGKR4BGt/dO2EvtuK9oUeMlvkkjhA==", "60db7208-2106-4ad4-99bd-383736bbbce9" });
        }
    }
}
