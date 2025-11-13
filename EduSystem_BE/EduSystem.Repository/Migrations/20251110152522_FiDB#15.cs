using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FiDB15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa column Rating nếu tồn tại (dùng SQL động)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Teachers' AND column_name = 'Rating'
                    ) THEN
                        ALTER TABLE ""Teachers"" DROP COLUMN ""Rating"";
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Thêm lại column Rating (nếu cần rollback)
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Teachers' AND column_name = 'Rating'
                    ) THEN
                        ALTER TABLE ""Teachers"" ADD COLUMN ""Rating"" numeric(3,2) NULL;
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da7299ea-0c57-4c64-a213-656c6498662a", "AQAAAAIAAYagAAAAEBawvA9GYmCm9FWlZXTpP+1LnbJSbYxtpEaZEAVTUEX72oWpwIk/BkU2qQu0eyZCmQ==", "3200a1d5-bb16-4383-9e98-413ae99f4dd5" });
        }
    }
}