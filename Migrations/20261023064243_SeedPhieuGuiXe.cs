using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyBaiDoXe_TI17A1ND.Migrations
{
    /// <inheritdoc />
    public partial class SeedPhieuGuiXe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PhieuGuiXes",
                columns: new[] { "MaPhieu", "DonGiaTheoGio", "MaPhuongTien", "MaViTri", "NgayDangKy", "ThanhTien", "ThoiGianDuKienRa", "ThoiGianDuKienVao", "ThoiGianRaThucTe", "ThoiGianVaoThucTe", "TrangThai" },
                values: new object[,]
                {
                    { 1, 3000m, 1, 1, new DateTime(2026, 8, 4, 8, 0, 0, 0, DateTimeKind.Unspecified), 9000m, new DateTime(2026, 8, 5, 10, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 10, 33, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 8, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 2, 3000m, 2, 1, new DateTime(2026, 8, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), 3000m, new DateTime(2026, 8, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 10, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 9, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 3, 20000m, 13, 13, new DateTime(2026, 8, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), 100000m, new DateTime(2026, 8, 18, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 18, 15, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 18, 10, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 4, 3000m, 9, 11, new DateTime(2026, 8, 24, 14, 0, 0, 0, DateTimeKind.Unspecified), 9000m, new DateTime(2026, 8, 25, 16, 5, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 25, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 25, 16, 8, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 25, 14, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 5, 3000m, 1, 1, new DateTime(2026, 9, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 12000m, new DateTime(2026, 9, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 12, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 8, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 6, 3000m, 10, 1, new DateTime(2026, 9, 5, 7, 30, 0, 0, DateTimeKind.Unspecified), 6000m, new DateTime(2026, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 6, 7, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 6, 9, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 6, 7, 33, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 7, 20000m, 14, 13, new DateTime(2026, 9, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), 60000m, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 12, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 9, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 8, 2000m, 18, 20, new DateTime(2026, 9, 13, 8, 0, 0, 0, DateTimeKind.Unspecified), 16000m, new DateTime(2026, 9, 14, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 16, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 14, 8, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 9, 3000m, 11, 6, new DateTime(2026, 9, 17, 17, 0, 0, 0, DateTimeKind.Unspecified), 3000m, new DateTime(2026, 9, 18, 17, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 18, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 18, 17, 48, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 18, 17, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 10, 3000m, 2, 1, new DateTime(2026, 9, 21, 8, 0, 0, 0, DateTimeKind.Unspecified), 30000m, new DateTime(2026, 9, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 22, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 22, 18, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 22, 8, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 11, 20000m, 15, 17, new DateTime(2026, 9, 26, 13, 0, 0, 0, DateTimeKind.Unspecified), 40000m, new DateTime(2026, 9, 27, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 27, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 27, 15, 3, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 27, 13, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 12, 3000m, 1, 1, new DateTime(2026, 10, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), 12000m, new DateTime(2026, 10, 3, 11, 20, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 3, 11, 23, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 3, 8, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 13, 3000m, 12, 12, new DateTime(2026, 10, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), 6000m, new DateTime(2026, 10, 8, 19, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 8, 19, 18, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 8, 18, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 14, 20000m, 13, 13, new DateTime(2026, 10, 11, 8, 30, 0, 0, DateTimeKind.Unspecified), 140000m, new DateTime(2026, 10, 12, 15, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 15, 33, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 8, 33, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 15, 2000m, 19, 20, new DateTime(2026, 10, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 2000m, new DateTime(2026, 10, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 15, 10, 33, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 15, 10, 3, 0, 0, DateTimeKind.Unspecified), "Đã lấy xe" },
                    { 16, 3000m, 7, 4, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 9, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đã hủy" },
                    { 17, 3000m, 8, 10, new DateTime(2026, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 5, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đã hủy" },
                    { 18, 20000m, 17, 16, new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đã hủy" },
                    { 19, 2000m, 19, 19, new DateTime(2026, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 14, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 14, 13, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đã hủy" },
                    { 20, 3000m, 3, 2, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 19, 8, 5, 0, 0, DateTimeKind.Unspecified), "Đang gửi" },
                    { 21, 3000m, 4, 9, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 19, 8, 5, 0, 0, DateTimeKind.Unspecified), "Đang gửi" },
                    { 22, 20000m, 15, 14, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 10, 19, 8, 5, 0, 0, DateTimeKind.Unspecified), "Đang gửi" },
                    { 23, 3000m, 5, 3, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chờ xác nhận" },
                    { 24, 3000m, 6, 3, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 5, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chờ xác nhận" },
                    { 25, 20000m, 16, 15, new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2026, 11, 6, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chờ xác nhận" }
                });

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 2,
                column: "TrangThai",
                value: "Đang sử dụng");

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 9,
                column: "TrangThai",
                value: "Đang sử dụng");

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 14,
                column: "TrangThai",
                value: "Đang sử dụng");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "PhieuGuiXes",
                keyColumn: "MaPhieu",
                keyValue: 25);

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 2,
                column: "TrangThai",
                value: "Còn trống");

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 9,
                column: "TrangThai",
                value: "Còn trống");

            migrationBuilder.UpdateData(
                table: "ViTriDoXes",
                keyColumn: "MaViTri",
                keyValue: 14,
                column: "TrangThai",
                value: "Còn trống");
        }
    }
}
