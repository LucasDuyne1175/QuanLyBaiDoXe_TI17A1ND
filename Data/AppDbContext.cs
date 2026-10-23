// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: DbContext, quan hệ và dữ liệu mẫu
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
    public DbSet<ChuPhuongTien> ChuPhuongTiens => Set<ChuPhuongTien>();
    public DbSet<PhuongTien> PhuongTiens => Set<PhuongTien>();
    public DbSet<PhieuGuiXe> PhieuGuiXes => Set<PhieuGuiXe>();

    private static readonly string[] HoTens =
    {
        "Nguyễn Văn A", "Trần Thị B", "Lê Văn C", "Phạm Thị D", "Hoàng Văn E",
        "Vũ Thị F", "Đặng Văn G", "Bùi Thị H", "Đỗ Văn I", "Ngô Thị K",
        "Dương Văn L", "Lý Thị M", "Phan Văn N", "Võ Thị O", "Trịnh Văn P"
    };

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ===== Chỉ mục, kiểu dữ liệu =====
        mb.Entity<TaiKhoan>().HasIndex(t => t.TenDangNhap).IsUnique();
        mb.Entity<LoaiPhuongTien>().HasIndex(l => l.TenLoaiPhuongTien).IsUnique();
        mb.Entity<LoaiPhuongTien>().Property(l => l.DonGiaTheoGio).HasColumnType("decimal(18,2)");
        mb.Entity<ViTriDoXe>().HasIndex(v => v.TenViTri).IsUnique();
        mb.Entity<PhuongTien>().HasIndex(p => p.BienSoXe).IsUnique();
        mb.Entity<PhieuGuiXe>().Property(p => p.DonGiaTheoGio).HasColumnType("decimal(18,2)");
        mb.Entity<PhieuGuiXe>().Property(p => p.ThanhTien).HasColumnType("decimal(18,2)");

        // ===== Quan hệ =====
        mb.Entity<ViTriDoXe>()
            .HasOne(v => v.LoaiPhuongTien).WithMany()
            .HasForeignKey(v => v.MaLoaiPhuongTien).OnDelete(DeleteBehavior.Restrict);

        // TaiKhoan 1-1 ChuPhuongTien
        mb.Entity<ChuPhuongTien>()
            .HasOne(c => c.TaiKhoan).WithOne()
            .HasForeignKey<ChuPhuongTien>(c => c.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);

        // ChuPhuongTien 1-n PhuongTien
        mb.Entity<PhuongTien>()
            .HasOne(p => p.ChuPhuongTien).WithMany(c => c.PhuongTiens)
            .HasForeignKey(p => p.MaChuPhuongTien).OnDelete(DeleteBehavior.Restrict);

        // LoaiPhuongTien 1-n PhuongTien
        mb.Entity<PhuongTien>()
            .HasOne(p => p.LoaiPhuongTien).WithMany()
            .HasForeignKey(p => p.MaLoaiPhuongTien).OnDelete(DeleteBehavior.Restrict);

        // PhuongTien 1-n PhieuGuiXe, ViTriDoXe 1-n PhieuGuiXe
        mb.Entity<PhieuGuiXe>()
            .HasOne(p => p.PhuongTien).WithMany()
            .HasForeignKey(p => p.MaPhuongTien).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<PhieuGuiXe>()
            .HasOne(p => p.ViTriDoXe).WithMany()
            .HasForeignKey(p => p.MaViTri).OnDelete(DeleteBehavior.Restrict);

        // ===== Dữ liệu mẫu =====
        SeedTaiKhoan(mb);
        SeedLoaiPhuongTien(mb);
        SeedViTri(mb);
        SeedChuVaPhuongTien(mb);
        SeedPhieuGuiXe(mb);
    }

    private static void SeedTaiKhoan(ModelBuilder mb)
    {
        var list = new List<TaiKhoan>
        {
            new() { MaTaiKhoan = 1, TenDangNhap = "admin", MatKhau = PasswordHelper.Hash("admin123"),
                    HoTen = "Quản trị viên", Email = "admin@baidoxe.vn", VaiTro = "Admin", TrangThai = true },
            new() { MaTaiKhoan = 3, TenDangNhap = "khoa", MatKhau = PasswordHelper.Hash("123456"),
                    HoTen = "Tài khoản bị khóa", Email = "khoa@mail.com", VaiTro = "KhachHang", TrangThai = false }
        };

        // khach1 -> id 2 (giữ như cũ); khach2..khach15 -> id 4..17
        for (int i = 1; i <= 15; i++)
        {
            list.Add(new TaiKhoan
            {
                MaTaiKhoan = i == 1 ? 2 : i + 2,
                TenDangNhap = $"khach{i}",
                MatKhau = PasswordHelper.Hash("123456"),
                HoTen = HoTens[i - 1],
                Email = $"khach{i}@mail.com",
                VaiTro = "KhachHang",
                TrangThai = true
            });
        }
        mb.Entity<TaiKhoan>().HasData(list);
    }

    private static void SeedLoaiPhuongTien(ModelBuilder mb)
    {
        mb.Entity<LoaiPhuongTien>().HasData(
            new LoaiPhuongTien { MaLoaiPhuongTien = 1, TenLoaiPhuongTien = "Xe máy", DonGiaTheoGio = 3000, MoTa = "Xe máy, xe tay ga", TrangThai = true },
            new LoaiPhuongTien { MaLoaiPhuongTien = 2, TenLoaiPhuongTien = "Ô tô", DonGiaTheoGio = 20000, MoTa = "Ô tô dưới 9 chỗ", TrangThai = true },
            new LoaiPhuongTien { MaLoaiPhuongTien = 3, TenLoaiPhuongTien = "Xe đạp điện", DonGiaTheoGio = 2000, MoTa = "Xe đạp, xe đạp điện", TrangThai = true });
    }

    private static void SeedViTri(ModelBuilder mb)
    {
        var viTris = new List<ViTriDoXe>();
        int id = 1;
        void Them(string khu, int soLuong, int tang, int maLoai, string moTa)
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
                    TrangThai = (ten == "A05" || ten == "C06") ? TrangThaiViTri.TamNgung
                              : (ten == "A02" || ten == "B01" || ten == "C02") ? TrangThaiViTri.DangSuDung
                              : TrangThaiViTri.ConTrong
                });
            }
        }
        Them("A", 8, 1, 1, "Vị trí đỗ xe máy tầng 1");
        Them("B", 4, 2, 1, "Vị trí đỗ xe máy tầng 2");
        Them("C", 6, 1, 2, "Vị trí đỗ ô tô");
        Them("D", 2, 1, 3, "Vị trí đỗ xe đạp điện");
        mb.Entity<ViTriDoXe>().HasData(viTris);
    }

    private static void SeedChuVaPhuongTien(ModelBuilder mb)
    {
        // 15 chủ phương tiện
        var chus = new List<ChuPhuongTien>();
        for (int i = 1; i <= 15; i++)
        {
            var ten = HoTens[i - 1];
            chus.Add(new ChuPhuongTien
            {
                MaChuPhuongTien = i,
                MaTaiKhoan = i == 1 ? 2 : i + 2,
                HoTen = ten,
                NgaySinh = new DateTime(1998 + i % 6, (i % 12) + 1, (i % 27) + 1),
                GioiTinh = ten.Contains("Thị") ? "Nữ" : "Nam",
                SoDienThoai = $"09012345{i:00}",
                Email = $"khach{i}@mail.com",
                DiaChi = "Hà Nội",
                NgayDangKy = new DateTime(2026, 9, 1),
                TrangThai = true
            });
        }
        mb.Entity<ChuPhuongTien>().HasData(chus);

        // 20 phương tiện: 1-12 xe máy, 13-17 ô tô, 18-20 xe đạp điện
        var mau = new[] { "Đen", "Trắng", "Đỏ", "Xanh" };
        var pts = new List<PhuongTien>();
        for (int i = 1; i <= 20; i++)
        {
            int loai = i <= 12 ? 1 : i <= 17 ? 2 : 3;
            int chu = i <= 12 ? i : i <= 17 ? i - 12 : i - 5;
            pts.Add(new PhuongTien
            {
                MaPhuongTien = i,
                MaChuPhuongTien = chu,
                MaLoaiPhuongTien = loai,
                BienSoXe = loai == 1 ? $"29A1-{12300 + i}" : loai == 2 ? $"30H-{45600 + i}" : $"29MD1-{78900 + i}",
                NhanHieu = loai == 1 ? "Honda" : loai == 2 ? "Toyota" : "VinFast",
                MauSac = mau[i % 4],
                TrangThai = true
            });
        }
        mb.Entity<PhuongTien>().HasData(pts);
    }
    private static void SeedPhieuGuiXe(ModelBuilder mb)
    {
        // Mã vị trí: A01-A08 = 1-8, B01-B04 = 9-12, C01-C06 = 13-18, D01-D02 = 19-20
        decimal Gia(int pt) => pt <= 12 ? 3000m : pt <= 17 ? 20000m : 2000m;
        var ds = new List<PhieuGuiXe>();
        int id = 1;

        // Phiếu đã hoàn tất: xe vào trễ 3 phút so với dự kiến, ở đúng số phút dự kiến
        void DaLayXe(int pt, int vt, DateTime vao, int phutGui)
        {
            var vaoTT = vao.AddMinutes(3);
            var raTT = vaoTT.AddMinutes(phutGui);
            int gio = TinhPhiHelper.SoGioTinhPhi(vaoTT, raTT);
            ds.Add(new PhieuGuiXe
            {
                MaPhieu = id++,
                MaPhuongTien = pt,
                MaViTri = vt,
                NgayDangKy = vao.AddDays(-1),
                ThoiGianDuKienVao = vao,
                ThoiGianDuKienRa = vao.AddMinutes(phutGui),
                ThoiGianVaoThucTe = vaoTT,
                ThoiGianRaThucTe = raTT,
                DonGiaTheoGio = Gia(pt),
                ThanhTien = TinhPhiHelper.ThanhTien(gio, Gia(pt)),
                TrangThai = TrangThaiPhieu.DaLayXe
            });
        }

        void Khac(int pt, int vt, DateTime dangKy, DateTime vao, DateTime ra, string tt, DateTime? vaoTT = null)
        {
            ds.Add(new PhieuGuiXe
            {
                MaPhieu = id++,
                MaPhuongTien = pt,
                MaViTri = vt,
                NgayDangKy = dangKy,
                ThoiGianDuKienVao = vao,
                ThoiGianDuKienRa = ra,
                ThoiGianVaoThucTe = vaoTT,
                DonGiaTheoGio = Gia(pt),
                TrangThai = tt
            });
        }

        // ----- 15 phiếu Đã lấy xe (tháng 8, 9, 10/2026) -----
        DaLayXe(1, 1, new DateTime(2026, 8, 5, 8, 0, 0), 150);
        DaLayXe(2, 1, new DateTime(2026, 8, 12, 9, 0, 0), 60);
        DaLayXe(13, 13, new DateTime(2026, 8, 18, 10, 0, 0), 300);
        DaLayXe(9, 11, new DateTime(2026, 8, 25, 14, 0, 0), 125);
        DaLayXe(1, 1, new DateTime(2026, 9, 2, 8, 0, 0), 240);
        DaLayXe(10, 1, new DateTime(2026, 9, 6, 7, 30, 0), 90);
        DaLayXe(14, 13, new DateTime(2026, 9, 10, 9, 0, 0), 180);
        DaLayXe(18, 20, new DateTime(2026, 9, 14, 8, 0, 0), 480);
        DaLayXe(11, 6, new DateTime(2026, 9, 18, 17, 0, 0), 45);
        DaLayXe(2, 1, new DateTime(2026, 9, 22, 8, 0, 0), 600);
        DaLayXe(15, 17, new DateTime(2026, 9, 27, 13, 0, 0), 120);
        DaLayXe(1, 1, new DateTime(2026, 10, 3, 8, 0, 0), 200);
        DaLayXe(12, 12, new DateTime(2026, 10, 8, 18, 0, 0), 75);
        DaLayXe(13, 13, new DateTime(2026, 10, 12, 8, 30, 0), 420);
        DaLayXe(19, 20, new DateTime(2026, 10, 15, 10, 0, 0), 30);

        // ----- 4 phiếu Đã hủy -----
        Khac(7, 4, new DateTime(2026, 9, 19), new DateTime(2026, 9, 20, 8, 0, 0), new DateTime(2026, 9, 20, 12, 0, 0), TrangThaiPhieu.DaHuy);
        Khac(8, 10, new DateTime(2026, 10, 4), new DateTime(2026, 10, 5, 9, 0, 0), new DateTime(2026, 10, 5, 11, 0, 0), TrangThaiPhieu.DaHuy);
        Khac(17, 16, new DateTime(2026, 10, 9), new DateTime(2026, 10, 10, 8, 0, 0), new DateTime(2026, 10, 10, 18, 0, 0), TrangThaiPhieu.DaHuy);
        Khac(19, 19, new DateTime(2026, 10, 13), new DateTime(2026, 10, 14, 13, 0, 0), new DateTime(2026, 10, 14, 15, 0, 0), TrangThaiPhieu.DaHuy);

        // ----- 3 phiếu Đang gửi (vị trí A02, B01, C02 là "Đang sử dụng") -----
        var vao19 = new DateTime(2026, 10, 19, 8, 0, 0);
        var ra30 = new DateTime(2026, 11, 30, 18, 0, 0);
        Khac(3, 2, new DateTime(2026, 10, 18), vao19, ra30, TrangThaiPhieu.DangGui, vao19.AddMinutes(5));
        Khac(4, 9, new DateTime(2026, 10, 18), vao19, ra30, TrangThaiPhieu.DangGui, vao19.AddMinutes(5));
        Khac(15, 14, new DateTime(2026, 10, 18), vao19, ra30, TrangThaiPhieu.DangGui, vao19.AddMinutes(5));

        // ----- 3 phiếu Chờ xác nhận (tháng 11/2026; 2 phiếu A03 nối tiếp nhau, không trùng) -----
        Khac(5, 3, new DateTime(2026, 10, 18), new DateTime(2026, 11, 5, 8, 0, 0), new DateTime(2026, 11, 5, 12, 0, 0), TrangThaiPhieu.ChoXacNhan);
        Khac(6, 3, new DateTime(2026, 10, 18), new DateTime(2026, 11, 5, 12, 0, 0), new DateTime(2026, 11, 5, 16, 0, 0), TrangThaiPhieu.ChoXacNhan);
        Khac(16, 15, new DateTime(2026, 10, 19), new DateTime(2026, 11, 6, 9, 0, 0), new DateTime(2026, 11, 6, 17, 0, 0), TrangThaiPhieu.ChoXacNhan);

        mb.Entity<PhieuGuiXe>().HasData(ds);
    }
}