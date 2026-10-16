// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: DbContext và dữ liệu mẫu
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Helpers;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
    public DbSet<LoaiPhuongTien> LoaiPhuongTiens => Set<LoaiPhuongTien>();
    public DbSet<ViTriDoXe> ViTriDoXes => Set<ViTriDoXe>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<TaiKhoan>().HasIndex(t => t.TenDangNhap).IsUnique();
        mb.Entity<LoaiPhuongTien>().HasIndex(l => l.TenLoaiPhuongTien).IsUnique();
        mb.Entity<LoaiPhuongTien>().Property(l => l.DonGiaTheoGio).HasColumnType("decimal(18,2)");

        mb.Entity<TaiKhoan>().HasData(
            new TaiKhoan
            {
                MaTaiKhoan = 1,
                TenDangNhap = "admin",
                MatKhau = PasswordHelper.Hash("admin123"),
                HoTen = "Quản trị viên",
                Email = "admin@baidoxe.vn",
                VaiTro = "Admin",
                TrangThai = true
            },
            new TaiKhoan
            {
                MaTaiKhoan = 2,
                TenDangNhap = "khach1",
                MatKhau = PasswordHelper.Hash("123456"),
                HoTen = "Nguyễn Văn A",
                Email = "khach1@mail.com",
                VaiTro = "KhachHang",
                TrangThai = true
            },
            new TaiKhoan
            {
                MaTaiKhoan = 3,
                TenDangNhap = "khoa",
                MatKhau = PasswordHelper.Hash("123456"),
                HoTen = "Tài khoản bị khóa",
                Email = "khoa@mail.com",
                VaiTro = "KhachHang",
                TrangThai = false
            });

        mb.Entity<LoaiPhuongTien>().HasData(
            new LoaiPhuongTien { MaLoaiPhuongTien = 1, TenLoaiPhuongTien = "Xe máy", DonGiaTheoGio = 3000, MoTa = "Xe máy, xe tay ga", TrangThai = true },
            new LoaiPhuongTien { MaLoaiPhuongTien = 2, TenLoaiPhuongTien = "Ô tô", DonGiaTheoGio = 20000, MoTa = "Ô tô dưới 9 chỗ", TrangThai = true },
            new LoaiPhuongTien { MaLoaiPhuongTien = 3, TenLoaiPhuongTien = "Xe đạp điện", DonGiaTheoGio = 2000, MoTa = "Xe đạp, xe đạp điện", TrangThai = true });

        mb.Entity<ViTriDoXe>().HasIndex(v => v.TenViTri).IsUnique();
        mb.Entity<ViTriDoXe>()
            .HasOne(v => v.LoaiPhuongTien)
            .WithMany()
            .HasForeignKey(v => v.MaLoaiPhuongTien)
            .OnDelete(DeleteBehavior.Restrict);

        // 20 vị trí mẫu: A (xe máy, tầng 1), B (xe máy, tầng 2), C (ô tô, tầng 1), D (xe đạp điện, tầng 1)
        var viTris = new List<ViTriDoXe>();
        int id = 1;
        void ThemViTri(string khu, int soLuong, int tang, int maLoai, string moTa)
        {
            for (int i = 1; i <= soLuong; i++)
            {
                var ten = $"{khu}{i:00}";
                viTris.Add(new ViTriDoXe
                {
                    MaViTri = id++,
                    TenViTri = ten,
                    KhuVuc = $"Khu {khu}",
                    Tang = tang,
                    MaLoaiPhuongTien = maLoai,
                    MoTa = moTa,
                    TrangThai = (ten == "A05" || ten == "C06") ? TrangThaiViTri.TamNgung : TrangThaiViTri.ConTrong
                });
            }
        }
        ThemViTri("A", 8, 1, 1, "Vị trí đỗ xe máy tầng 1");
        ThemViTri("B", 4, 2, 1, "Vị trí đỗ xe máy tầng 2");
        ThemViTri("C", 6, 1, 2, "Vị trí đỗ ô tô");
        ThemViTri("D", 2, 1, 3, "Vị trí đỗ xe đạp điện");
        mb.Entity<ViTriDoXe>().HasData(viTris);
    }
}