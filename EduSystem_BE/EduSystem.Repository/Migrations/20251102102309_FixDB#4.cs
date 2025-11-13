using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c41abdb-d386-42ad-a754-390b844c18a9", "AQAAAAIAAYagAAAAEFUbBYlFc8kSGysRz+FeqLr50u+tCBnSFFXjSvGKR4BGt/dO2EvtuK9oUeMlvkkjhA==", "60db7208-2106-4ad4-99bd-383736bbbce9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05da2f8c-088f-4778-9d2e-77513b4f4ee6", "AQAAAAIAAYagAAAAEKCbsHfAAo3oSyWhT/ahT4CfF4CeKE40KSLnrq2Cy8glz4EG/FxHjm5F35HDn4LBLg==", "0c03c4c6-2f6b-40b0-90bb-26ab547aed5c" });
        }
    }
}
