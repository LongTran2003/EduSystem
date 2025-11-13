using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixDB18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm CreatedBy nếu chưa có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'CreatedBy'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" ADD COLUMN ""CreatedBy"" text NULL;
                    END IF;
                END $$;
            ");

            // Thêm CreatedTime nếu chưa có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'CreatedTime'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" ADD COLUMN ""CreatedTime"" timestamp with time zone NULL;
                    END IF;
                END $$;
            ");

            // Thêm Status nếu chưa có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'Status'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" ADD COLUMN ""Status"" text NULL;
                    END IF;
                END $$;
            ");

            // Thêm UpdatedBy nếu chưa có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'UpdatedBy'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" ADD COLUMN ""UpdatedBy"" text NULL;
                    END IF;
                END $$;
            ");

            // Thêm UpdatedTime nếu chưa có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'UpdatedTime'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" ADD COLUMN ""UpdatedTime"" timestamp with time zone NULL;
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2c86037-ad54-41a6-8e13-c4bc1363342d", "AQAAAAIAAYagAAAAEBxLbPvlgbmCQfKjftMOD0mSWyLs3C6+nn1uWcwtnQoVtItVt/epyc3lN9ZdDm1gKw==", "e554e44d-5540-42fb-a835-1987f98df22a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa CreatedBy nếu có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'CreatedBy'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" DROP COLUMN ""CreatedBy"";
                    END IF;
                END $$;
            ");

            // Xóa CreatedTime nếu có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'CreatedTime'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" DROP COLUMN ""CreatedTime"";
                    END IF;
                END $$;
            ");

            // Xóa Status nếu có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'Status'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" DROP COLUMN ""Status"";
                    END IF;
                END $$;
            ");

            // Xóa UpdatedBy nếu có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'UpdatedBy'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" DROP COLUMN ""UpdatedBy"";
                    END IF;
                END $$;
            ");

            // Xóa UpdatedTime nếu có
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'StudentProgresses' AND column_name = 'UpdatedTime'
                    ) THEN
                        ALTER TABLE ""StudentProgresses"" DROP COLUMN ""UpdatedTime"";
                    END IF;
                END $$;
            ");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EduSystem-Admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d61e59e-8b5f-41dc-ab10-93f0138b342a", "AQAAAAIAAYagAAAAENsJrkHoBXgrcbHOJzxbp9rDBhJEH0YKc5+WJvnWo8oWgl3HEBUVmn4y6IOYgDbX0A==", "43c2a2c8-b8d4-4ec2-aba1-3328bd4ea472" });
        }
    }
}