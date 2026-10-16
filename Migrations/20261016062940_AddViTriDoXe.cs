using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyBaiDoXe_TI17A1ND.Migrations
{
    /// <inheritdoc />
    public partial class AddViTriDoXe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViTriDoXes",
                columns: table => new
                {
                    MaViTri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenViTri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaLoaiPhuongTien = table.Column<int>(type: "int", nullable: false),
                    KhuVuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tang = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViTriDoXes", x => x.MaViTri);
                    table.ForeignKey(
                        name: "FK_ViTriDoXes_LoaiPhuongTiens_MaLoaiPhuongTien",
                        column: x => x.MaLoaiPhuongTien,
                        principalTable: "LoaiPhuongTiens",
                        principalColumn: "MaLoaiPhuongTien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ViTriDoXes",
                columns: new[] { "MaViTri", "KhuVuc", "MaLoaiPhuongTien", "MoTa", "Tang", "TenViTri", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A01", "Còn trống" },
                    { 2, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A02", "Còn trống" },
                    { 3, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A03", "Còn trống" },
                    { 4, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A04", "Còn trống" },
                    { 5, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A05", "Tạm ngừng" },
                    { 6, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A06", "Còn trống" },
                    { 7, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A07", "Còn trống" },
                    { 8, "Khu A", 1, "Vị trí đỗ xe máy tầng 1", 1, "A08", "Còn trống" },
                    { 9, "Khu B", 1, "Vị trí đỗ xe máy tầng 2", 2, "B01", "Còn trống" },
                    { 10, "Khu B", 1, "Vị trí đỗ xe máy tầng 2", 2, "B02", "Còn trống" },
                    { 11, "Khu B", 1, "Vị trí đỗ xe máy tầng 2", 2, "B03", "Còn trống" },
                    { 12, "Khu B", 1, "Vị trí đỗ xe máy tầng 2", 2, "B04", "Còn trống" },
                    { 13, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C01", "Còn trống" },
                    { 14, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C02", "Còn trống" },
                    { 15, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C03", "Còn trống" },
                    { 16, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C04", "Còn trống" },
                    { 17, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C05", "Còn trống" },
                    { 18, "Khu C", 2, "Vị trí đỗ ô tô", 1, "C06", "Tạm ngừng" },
                    { 19, "Khu D", 3, "Vị trí đỗ xe đạp điện", 1, "D01", "Còn trống" },
                    { 20, "Khu D", 3, "Vị trí đỗ xe đạp điện", 1, "D02", "Còn trống" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViTriDoXes_MaLoaiPhuongTien",
                table: "ViTriDoXes",
                column: "MaLoaiPhuongTien");

            migrationBuilder.CreateIndex(
                name: "IX_ViTriDoXes_TenViTri",
                table: "ViTriDoXes",
                column: "TenViTri",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViTriDoXes");
        }
    }
}
