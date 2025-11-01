using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeachingExperience",
                table: "Teachers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05da2f8c-088f-4778-9d2e-77513b4f4ee6", "AQAAAAIAAYagAAAAEKCbsHfAAo3oSyWhT/ahT4CfF4CeKE40KSLnrq2Cy8glz4EG/FxHjm5F35HDn4LBLg==", "0c03c4c6-2f6b-40b0-90bb-26ab547aed5c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeachingExperience",
                table: "Teachers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46e63db6-49cb-4915-8980-2c2323194381", "AQAAAAIAAYagAAAAED2fTUg5h3Z/Z5jxLTTIUqBYJvQes91aNOi91puYpN+pZunKbQaC5H1Vp2mptwce0Q==", "fa5c7679-c050-4396-a7a1-b7226a03acf2" });
        }
    }
}
