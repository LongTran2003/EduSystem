using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FiDB16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa column Specialization nếu tồn tại (dùng SQL động)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Teachers' AND column_name = 'Specialization'
                    ) THEN
                        ALTER TABLE ""Teachers"" DROP COLUMN ""Specialization"";
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20f6a7f8-b32d-4b0f-ac4d-85a13fdfd99f", "AQAAAAIAAYagAAAAEC1nTk09l5/2VXe2rk4jBtcrYaZB6TvUSx4rPNVlQIyzTON+IukU2eICO+ncHtZvNw==", "15df3a25-a0cf-42cd-a4d1-7dc571573a24" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Thêm lại column Specialization nếu cần rollback
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Teachers' AND column_name = 'Specialization'
                    ) THEN
                        ALTER TABLE ""Teachers"" ADD COLUMN ""Specialization"" character varying(100) NULL;
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9fcf4e71-8cfb-4d26-b286-35430a3ad367", "AQAAAAIAAYagAAAAEE6i2VdMwhZLmYeE1dGQWBphpkBpOUiK6iXUDwm7k7mm7O+BiZ13Lu15y103SViz3A==", "9c7de1bf-8cba-4622-9766-c71e1d5e7150" });
        }
    }
}