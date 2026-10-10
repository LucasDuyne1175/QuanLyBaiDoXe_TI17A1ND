using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyBaiDoXe_TI17A1ND.Migrations
{
    /// <inheritdoc />
    public partial class InitModule1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiPhuongTiens",
                columns: table => new
                {
                    MaLoaiPhuongTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiPhuongTien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonGiaTheoGio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhuongTiens", x => x.MaLoaiPhuongTien);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.MaTaiKhoan);
                });

            migrationBuilder.InsertData(
                table: "LoaiPhuongTiens",
                columns: new[] { "MaLoaiPhuongTien", "DonGiaTheoGio", "MoTa", "TenLoaiPhuongTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, 3000m, "Xe máy, xe tay ga", "Xe máy", true },
                    { 2, 20000m, "Ô tô dưới 9 chỗ", "Ô tô", true },
                    { 3, 2000m, "Xe đạp, xe đạp điện", "Xe đạp điện", true }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoans",
                columns: new[] { "MaTaiKhoan", "Email", "HoTen", "MatKhau", "TenDangNhap", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { 1, "admin@baidoxe.vn", "Quản trị viên", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "admin", true, "Admin" },
                    { 2, "khach1@mail.com", "Nguyễn Văn A", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khach1", true, "KhachHang" },
                    { 3, "khoa@mail.com", "Tài khoản bị khóa", "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", "khoa", false, "KhachHang" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoaiPhuongTiens_TenLoaiPhuongTien",
                table: "LoaiPhuongTiens",
                column: "TenLoaiPhuongTien",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_TenDangNhap",
                table: "TaiKhoans",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoaiPhuongTiens");

            migrationBuilder.DropTable(
                name: "TaiKhoans");
        }
    }
}
