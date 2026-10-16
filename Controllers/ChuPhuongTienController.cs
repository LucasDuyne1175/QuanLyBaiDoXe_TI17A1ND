// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quản lý chủ phương tiện và hồ sơ cá nhân
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

public class ChuPhuongTienController : Controller
{
    private readonly AppDbContext _db;
    public ChuPhuongTienController(AppDbContext db) => _db = db;

    // ===== ADMIN =====
    [AuthorizeRole("Admin")]
    public async Task<IActionResult> Index(string? keyword, bool? hoatDong)
    {
        var q = _db.ChuPhuongTiens.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(c => c.HoTen.Contains(keyword)
                          || (c.SoDienThoai != null && c.SoDienThoai.Contains(keyword))
                          || (c.Email != null && c.Email.Contains(keyword)));
        }
        if (hoatDong.HasValue) q = q.Where(c => c.TrangThai == hoatDong.Value);

        ViewBag.Keyword = keyword;
        ViewBag.HoatDong = hoatDong;
        return View(await q.OrderBy(c => c.HoTen).ToListAsync());
    }

    [AuthorizeRole("Admin")]
    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.ChuPhuongTiens.AsNoTracking()
            .Include(c => c.TaiKhoan)
            .Include(c => c.PhuongTiens).ThenInclude(p => p.LoaiPhuongTien)
            .FirstOrDefaultAsync(c => c.MaChuPhuongTien == id);
        return item == null ? NotFound() : View(item);
    }

    [AuthorizeRole("Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DoiTrangThai(int id)
    {
        var item = await _db.ChuPhuongTiens.FindAsync(id);
        if (item == null) return NotFound();

        item.TrangThai = !item.TrangThai;
        await _db.SaveChangesAsync();
        TempData["Msg"] = item.TrangThai ? "Đã kích hoạt chủ phương tiện." : "Đã ngừng hoạt động chủ phương tiện.";
        return RedirectToAction(nameof(Index));
    }

    // ===== KHÁCH HÀNG: chỉ xem/sửa hồ sơ của chính mình =====
    [AuthorizeRole("KhachHang")]
    [HttpGet]
    public async Task<IActionResult> HoSo()
    {
        var chu = await LayHoacTaoChuXe();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        return View(new HoSoViewModel
        {
            HoTen = chu.HoTen,
            NgaySinh = chu.NgaySinh,
            GioiTinh = chu.GioiTinh,
            SoDienThoai = chu.SoDienThoai,
            Email = chu.Email,
            DiaChi = chu.DiaChi
        });
    }

    [AuthorizeRole("KhachHang")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> HoSo(HoSoViewModel vm)
    {
        if (vm.NgaySinh.HasValue &&
            (vm.NgaySinh.Value.Date > DateTime.Today || vm.NgaySinh.Value.Year < 1900))
            ModelState.AddModelError(nameof(vm.NgaySinh), "Ngày sinh không hợp lệ.");
        if (!ModelState.IsValid) return View(vm);

        var chu = await LayHoacTaoChuXe();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        chu.HoTen = vm.HoTen;
        chu.NgaySinh = vm.NgaySinh;
        chu.GioiTinh = vm.GioiTinh;
        chu.SoDienThoai = vm.SoDienThoai;
        chu.Email = vm.Email;
        chu.DiaChi = vm.DiaChi;

        // Đồng bộ tên hiển thị của tài khoản
        var tk = await _db.TaiKhoans.FindAsync(chu.MaTaiKhoan);
        if (tk != null) { tk.HoTen = vm.HoTen; tk.Email = vm.Email ?? tk.Email; }

        await _db.SaveChangesAsync();
        HttpContext.Session.SetString("HoTen", vm.HoTen);
        TempData["Msg"] = "Cập nhật thông tin cá nhân thành công.";
        return RedirectToAction(nameof(HoSo));
    }

    // Lấy chủ xe của tài khoản đang đăng nhập; chưa có thì tạo (cho tài khoản đăng ký trước đó)
    private async Task<ChuPhuongTien?> LayHoacTaoChuXe()
    {
        var maTk = HttpContext.Session.GetInt32("MaTaiKhoan");
        if (maTk == null) return null;

        var chu = await _db.ChuPhuongTiens.FirstOrDefaultAsync(c => c.MaTaiKhoan == maTk.Value);
        if (chu != null) return chu;

        var tk = await _db.TaiKhoans.FindAsync(maTk.Value);
        if (tk == null) return null;

        chu = new ChuPhuongTien
        {
            MaTaiKhoan = tk.MaTaiKhoan,
            HoTen = tk.HoTen,
            Email = tk.Email,
            NgayDangKy = DateTime.Now,
            TrangThai = true
        };
        _db.ChuPhuongTiens.Add(chu);
        await _db.SaveChangesAsync();
        return chu;
    }
}