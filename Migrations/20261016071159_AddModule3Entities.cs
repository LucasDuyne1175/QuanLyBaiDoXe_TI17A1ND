using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyBaiDoXe_TI17A1ND.Migrations
{
    /// <inheritdoc />
    public partial class AddModule3Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuPhuongTiens",
                columns: table => new
                {
                    MaChuPhuongTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuPhuongTiens", x => x.MaChuPhuongTien);
                    table.ForeignKey(
                        name: "FK_ChuPhuongTiens_TaiKhoans_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhuongTiens",
                columns: table => new
                {
                    MaPhuongTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChuPhuongTien = table.Column<int>(type: "int", nullable: false),
                    MaLoaiPhuongTien = table.Column<int>(type: "int", nullable: false),
                    BienSoXe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NhanHieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongTiens", x => x.MaPhuongTien);
                    table.ForeignKey(
                        name: "FK_PhuongTiens_ChuPhuongTiens_MaChuPhuongTien",
                        column: x => x.MaChuPhuongTien,
                        principalTable: "ChuPhuongTiens",
                        principalColumn: "MaChuPhuongTien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhuongTiens_LoaiPhuongTiens_MaLoaiPhuongTien",
                        column: x => x.MaLoaiPhuongTien,
                        principalTable: "LoaiPhuongTiens",
                        principalColumn: "MaLoaiPhuongTien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuGuiXes",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhuongTien = table.Column<int>(type: "int", nullable: false),
                    MaViTri = table.Column<int>(type: "int", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuKienVao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuKienRa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianVaoThucTe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianRaThucTe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DonGiaTheoGio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuGuiXes", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PhieuGuiXes_PhuongTiens_MaPhuongTien",
                        column: x => x.MaPhuongTien,
                        principalTable: "PhuongTiens",
                        principalColumn: "MaPhuongTien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhieuGuiXes_ViTriDoXes_MaViTri",
                        column: x => x.MaViTri,
                        principalTable: "ViTriDoXes",
                        principalColumn: "MaViTri",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ChuPhuongTiens",
                columns: new[] { "MaChuPhuongTien", "DiaChi", "Email", "GioiTinh", "HoTen", "MaTaiKhoan", "NgayDangKy", "NgaySinh", "SoDienThoai", "TrangThai" },
                values: new object[] { 1, "Hà Nội", "khach1@mail.com", "Nam", "Nguyễn Văn A", 2, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234501", true });

            migrationBuilder.InsertData(
                table: "TaiKhoans",
                columns: new[] { "MaTaiKhoan", "Email", "HoTen", "MatKhau", "TenDangNhap", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { 4, "khach2@mail.com", "Trần Thị B", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach2", true, "KhachHang" },
                    { 5, "khach3@mail.com", "Lê Văn C", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach3", true, "KhachHang" },
                    { 6, "khach4@mail.com", "Phạm Thị D", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach4", true, "KhachHang" },
                    { 7, "khach5@mail.com", "Hoàng Văn E", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach5", true, "KhachHang" },
                    { 8, "khach6@mail.com", "Vũ Thị F", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach6", true, "KhachHang" },
                    { 9, "khach7@mail.com", "Đặng Văn G", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach7", true, "KhachHang" },
                    { 10, "khach8@mail.com", "Bùi Thị H", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach8", true, "KhachHang" },
                    { 11, "khach9@mail.com", "Đỗ Văn I", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach9", true, "KhachHang" },
                    { 12, "khach10@mail.com", "Ngô Thị K", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach10", true, "KhachHang" },
                    { 13, "khach11@mail.com", "Dương Văn L", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach11", true, "KhachHang" },
                    { 14, "khach12@mail.com", "Lý Thị M", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach12", true, "KhachHang" },
                    { 15, "khach13@mail.com", "Phan Văn N", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach13", true, "KhachHang" },
                    { 16, "khach14@mail.com", "Võ Thị O", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach14", true, "KhachHang" },
                    { 17, "khach15@mail.com", "Trịnh Văn P", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach15", true, "KhachHang" }
                });

            migrationBuilder.InsertData(
                table: "ChuPhuongTiens",
                columns: new[] { "MaChuPhuongTien", "DiaChi", "Email", "GioiTinh", "HoTen", "MaTaiKhoan", "NgayDangKy", "NgaySinh", "SoDienThoai", "TrangThai" },
                values: new object[,]
                {
                    { 2, "Hà Nội", "khach2@mail.com", "Nữ", "Trần Thị B", 4, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234502", true },
                    { 3, "Hà Nội", "khach3@mail.com", "Nam", "Lê Văn C", 5, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234503", true },
                    { 4, "Hà Nội", "khach4@mail.com", "Nữ", "Phạm Thị D", 6, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2002, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234504", true },
                    { 5, "Hà Nội", "khach5@mail.com", "Nam", "Hoàng Văn E", 7, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2003, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234505", true },
                    { 6, "Hà Nội", "khach6@mail.com", "Nữ", "Vũ Thị F", 8, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234506", true },
                    { 7, "Hà Nội", "khach7@mail.com", "Nam", "Đặng Văn G", 9, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234507", true },
                    { 8, "Hà Nội", "khach8@mail.com", "Nữ", "Bùi Thị H", 10, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234508", true },
                    { 9, "Hà Nội", "khach9@mail.com", "Nam", "Đỗ Văn I", 11, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234509", true },
                    { 10, "Hà Nội", "khach10@mail.com", "Nữ", "Ngô Thị K", 12, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2002, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234510", true },
                    { 11, "Hà Nội", "khach11@mail.com", "Nam", "Dương Văn L", 13, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2003, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234511", true },
                    { 12, "Hà Nội", "khach12@mail.com", "Nữ", "Lý Thị M", 14, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234512", true },
                    { 13, "Hà Nội", "khach13@mail.com", "Nam", "Phan Văn N", 15, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234513", true },
                    { 14, "Hà Nội", "khach14@mail.com", "Nữ", "Võ Thị O", 16, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234514", true },
                    { 15, "Hà Nội", "khach15@mail.com", "Nam", "Trịnh Văn P", 17, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "0901234515", true }
                });

            migrationBuilder.InsertData(
                table: "PhuongTiens",
                columns: new[] { "MaPhuongTien", "BienSoXe", "MaChuPhuongTien", "MaLoaiPhuongTien", "MauSac", "MoTa", "NhanHieu", "TrangThai" },
                values: new object[,]
                {
                    { 1, "29A1-12301", 1, 1, "Trắng", null, "Honda", true },
                    { 13, "30H-45613", 1, 2, "Trắng", null, "Toyota", true },
                    { 2, "29A1-12302", 2, 1, "Đỏ", null, "Honda", true },
                    { 3, "29A1-12303", 3, 1, "Xanh", null, "Honda", true },
                    { 4, "29A1-12304", 4, 1, "Đen", null, "Honda", true },
                    { 5, "29A1-12305", 5, 1, "Trắng", null, "Honda", true },
                    { 6, "29A1-12306", 6, 1, "Đỏ", null, "Honda", true },
                    { 7, "29A1-12307", 7, 1, "Xanh", null, "Honda", true },
                    { 8, "29A1-12308", 8, 1, "Đen", null, "Honda", true },
                    { 9, "29A1-12309", 9, 1, "Trắng", null, "Honda", true },
                    { 10, "29A1-12310", 10, 1, "Đỏ", null, "Honda", true },
                    { 11, "29A1-12311", 11, 1, "Xanh", null, "Honda", true },
                    { 12, "29A1-12312", 12, 1, "Đen", null, "Honda", true },
                    { 14, "30H-45614", 2, 2, "Đỏ", null, "Toyota", true },
                    { 15, "30H-45615", 3, 2, "Xanh", null, "Toyota", true },
                    { 16, "30H-45616", 4, 2, "Đen", null, "Toyota", true },
                    { 17, "30H-45617", 5, 2, "Trắng", null, "Toyota", true },
                    { 18, "29MD1-78918", 13, 3, "Đỏ", null, "VinFast", true },
                    { 19, "29MD1-78919", 14, 3, "Xanh", null, "VinFast", true },
                    { 20, "29MD1-78920", 15, 3, "Đen", null, "VinFast", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuPhuongTiens_MaTaiKhoan",
                table: "ChuPhuongTiens",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuGuiXes_MaPhuongTien",
                table: "PhieuGuiXes",
                column: "MaPhuongTien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuGuiXes_MaViTri",
                table: "PhieuGuiXes",
                column: "MaViTri");

            migrationBuilder.CreateIndex(
                name: "IX_PhuongTiens_BienSoXe",
                table: "PhuongTiens",
                column: "BienSoXe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhuongTiens_MaChuPhuongTien",
                table: "PhuongTiens",
                column: "MaChuPhuongTien");

            migrationBuilder.CreateIndex(
                name: "IX_PhuongTiens_MaLoaiPhuongTien",
                table: "PhuongTiens",
                column: "MaLoaiPhuongTien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuGuiXes");

            migrationBuilder.DropTable(
                name: "PhuongTiens");

            migrationBuilder.DropTable(
                name: "ChuPhuongTiens");

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TaiKhoans",
                keyColumn: "MaTaiKhoan",
                keyValue: 17);
        }
    }
}
