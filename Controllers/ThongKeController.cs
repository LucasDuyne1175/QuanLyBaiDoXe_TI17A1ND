// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Thống kê bằng LINQ (Count, Sum, GroupBy, OrderBy, OrderByDescending)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole("Admin")]
public class ThongKeController : Controller
{
    private readonly AppDbContext _db;
    public ThongKeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var vm = new ThongKeViewModel();

        // 1. Số phương tiện theo từng loại: GroupBy theo tên loại, đếm số phần tử mỗi nhóm
        vm.PhuongTienTheoLoai = await _db.PhuongTiens
            .GroupBy(p => p.LoaiPhuongTien!.TenLoaiPhuongTien)
            .Select(g => new DongThongKe { Ten = g.Key, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // 2. Số vị trí theo từng trạng thái
        vm.ViTriTheoTrangThai = await _db.ViTriDoXes
            .GroupBy(v => v.TrangThai)
            .Select(g => new DongThongKe { Ten = g.Key, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // 3. Số phiếu theo từng trạng thái
        vm.PhieuTheoTrangThai = await _db.PhieuGuiXes
            .GroupBy(p => p.TrangThai)
            .Select(g => new DongThongKe { Ten = g.Key, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // "Lượt gửi" = phiếu mà xe đã thực sự vào bãi (Đang gửi hoặc Đã lấy xe)
        var luotGui = _db.PhieuGuiXes.Where(p => p.ThoiGianVaoThucTe != null);

        // 4. Số lượt gửi theo từng loại phương tiện
        vm.LuotTheoLoai = await luotGui
            .GroupBy(p => p.PhuongTien!.LoaiPhuongTien!.TenLoaiPhuongTien)
            .Select(g => new DongThongKe { Ten = g.Key, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // 5. Số lượt gửi theo từng vị trí (sắp giảm dần theo số lượt)
        vm.LuotTheoViTri = await luotGui
            .GroupBy(p => p.ViTriDoXe!.TenViTri)
            .Select(g => new DongThongKe { Ten = g.Key, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // 5b. Vị trí có số lượt sử dụng nhiều nhất = phần tử đầu của danh sách đã sắp giảm dần
        vm.ViTriNhieuNhat = vm.LuotTheoViTri.FirstOrDefault();

        // 6. Số lượt gửi theo từng chủ phương tiện (nhóm theo mã chủ để tránh trùng tên)
        vm.LuotTheoChuXe = await luotGui
            .GroupBy(p => new { p.PhuongTien!.MaChuPhuongTien, p.PhuongTien.ChuPhuongTien!.HoTen })
            .Select(g => new DongThongKe { Ten = g.Key.HoTen, SoLuong = g.Count() })
            .OrderByDescending(x => x.SoLuong).ThenBy(x => x.Ten)
            .ToListAsync();

        // 7. Số lượt gửi theo tháng (theo tháng xe vào)
        var luotThang = await luotGui
            .GroupBy(p => new { p.ThoiGianVaoThucTe!.Value.Year, p.ThoiGianVaoThucTe.Value.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, SoLuot = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        // 8. Doanh thu theo tháng: chỉ phiếu Đã lấy xe, tổng ThanhTien theo tháng xe ra
        var doanhThu = await _db.PhieuGuiXes
            .Where(p => p.TrangThai == TrangThaiPhieu.DaLayXe && p.ThoiGianRaThucTe != null)
            .GroupBy(p => new { p.ThoiGianRaThucTe!.Value.Year, p.ThoiGianRaThucTe.Value.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, SoPhieu = g.Count(), TongTien = g.Sum(p => p.ThanhTien ?? 0m) })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        // Ghép 2 kết quả theo tháng (xử lý trong bộ nhớ vì chỉ vài dòng)
        var cacThang = luotThang.Select(x => (x.Year, x.Month))
            .Union(doanhThu.Select(x => (x.Year, x.Month)))
            .OrderBy(x => x.Year).ThenBy(x => x.Month);

        vm.TheoThang = cacThang.Select(t => new DongThang
        {
            Thang = $"{t.Month:00}/{t.Year}",
            SoLuot = luotThang.FirstOrDefault(x => x.Year == t.Year && x.Month == t.Month)?.SoLuot ?? 0,
            DoanhThu = doanhThu.FirstOrDefault(x => x.Year == t.Year && x.Month == t.Month)?.TongTien ?? 0m
        }).ToList();

        vm.TongDoanhThu = vm.TheoThang.Sum(t => t.DoanhThu);
        return View(vm);
    }
}