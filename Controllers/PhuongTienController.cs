// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quản lý phương tiện
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Helpers;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole] // Admin và Khách hàng; quyền chi tiết kiểm tra trong từng action
public class PhuongTienController : Controller
{
    private readonly AppDbContext _db;
    public PhuongTienController(AppDbContext db) => _db = db;

    private bool LaAdmin => HttpContext.Session.GetString("VaiTro") == "Admin";

    // Chủ xe của khách đang đăng nhập (Admin không có)
    private async Task<ChuPhuongTien?> ChuHienTai()
    {
        var maTk = HttpContext.Session.GetInt32("MaTaiKhoan");
        return maTk == null ? null : await _db.LayHoacTaoChuXe(maTk.Value);
    }

    private bool CoQuyen(PhuongTien p, ChuPhuongTien? chu) =>
        LaAdmin || (chu != null && p.MaChuPhuongTien == chu.MaChuPhuongTien);

    private IActionResult TuChoi() => RedirectToAction("AccessDenied", "TaiKhoan");

    // ===== DANH SÁCH =====
    public async Task<IActionResult> Index(string? keyword, int? maLoai, bool? hoatDong)
    {
        var q = _db.PhuongTiens.AsNoTracking()
            .Include(p => p.LoaiPhuongTien).Include(p => p.ChuPhuongTien).AsQueryable();

        if (!LaAdmin)
        {
            var chu = await ChuHienTai();
            if (chu == null) return RedirectToAction("Login", "TaiKhoan");
            q = q.Where(p => p.MaChuPhuongTien == chu.MaChuPhuongTien); // khách chỉ thấy xe của mình
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(p => p.BienSoXe.Contains(keyword)
                          || (p.NhanHieu != null && p.NhanHieu.Contains(keyword))
                          || p.ChuPhuongTien!.HoTen.Contains(keyword));
        }
        if (maLoai.HasValue) q = q.Where(p => p.MaLoaiPhuongTien == maLoai.Value);
        if (hoatDong.HasValue) q = q.Where(p => p.TrangThai == hoatDong.Value);

        var loais = await _db.LoaiPhuongTiens.OrderBy(l => l.TenLoaiPhuongTien).ToListAsync();
        ViewBag.LoaiList = new SelectList(loais, "MaLoaiPhuongTien", "TenLoaiPhuongTien", maLoai);
        ViewBag.Keyword = keyword;
        ViewBag.HoatDong = hoatDong;
        ViewBag.LaAdmin = LaAdmin;

        return View(await q.OrderBy(p => p.BienSoXe).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.PhuongTiens.AsNoTracking()
            .Include(p => p.LoaiPhuongTien).Include(p => p.ChuPhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhuongTien == id);
        if (item == null) return NotFound();

        var chu = LaAdmin ? null : await ChuHienTai();
        return CoQuyen(item, chu) ? View(item) : TuChoi();
    }

    // ===== THÊM =====
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapDanhSach();
        return View(new PhuongTien());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhuongTien model)
    {
        model.BienSoXe = (model.BienSoXe ?? "").Trim().ToUpper();

        if (LaAdmin)
        {
            if (!await _db.ChuPhuongTiens.AnyAsync(c => c.MaChuPhuongTien == model.MaChuPhuongTien))
                ModelState.AddModelError(nameof(model.MaChuPhuongTien), "Vui lòng chọn chủ phương tiện.");
        }
        else
        {
            var chu = await ChuHienTai();
            if (chu == null) return RedirectToAction("Login", "TaiKhoan");
            if (!chu.TrangThai)
                ModelState.AddModelError("", "Hồ sơ chủ phương tiện của bạn đang ngừng hoạt động.");
            model.MaChuPhuongTien = chu.MaChuPhuongTien; // luôn lấy từ server, không tin dữ liệu form
        }

        await KiemTraHopLe(model);
        if (!ModelState.IsValid)
        {
            await NapDanhSach(model.MaLoaiPhuongTien, model.MaChuPhuongTien);
            return View(model);
        }

        model.MaPhuongTien = 0;
        _db.PhuongTiens.Add(model);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Thêm phương tiện thành công.";
        return RedirectToAction(nameof(Index));
    }

    // ===== SỬA =====
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.PhuongTiens.Include(p => p.ChuPhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhuongTien == id);
        if (item == null) return NotFound();

        var chu = LaAdmin ? null : await ChuHienTai();
        if (!CoQuyen(item, chu)) return TuChoi();

        await NapDanhSach(item.MaLoaiPhuongTien);
        ViewBag.TenChu = item.ChuPhuongTien?.HoTen;
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PhuongTien model)
    {
        var item = await _db.PhuongTiens.Include(p => p.ChuPhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhuongTien == id);
        if (item == null) return NotFound();

        var chu = LaAdmin ? null : await ChuHienTai();
        if (!CoQuyen(item, chu)) return TuChoi(); // kiểm tra theo dữ liệu trong DB

        model.MaPhuongTien = id;
        model.MaChuPhuongTien = item.MaChuPhuongTien; // không cho đổi chủ
        model.BienSoXe = (model.BienSoXe ?? "").Trim().ToUpper();

        await KiemTraHopLe(model);
        if (model.MaLoaiPhuongTien != item.MaLoaiPhuongTien && await CoPhieuHoatDong(id))
            ModelState.AddModelError(nameof(model.MaLoaiPhuongTien),
                "Không thể đổi loại: phương tiện đang có lượt gửi xe hoạt động.");

        if (!ModelState.IsValid)
        {
            await NapDanhSach(model.MaLoaiPhuongTien);
            ViewBag.TenChu = item.ChuPhuongTien?.HoTen;
            return View(model);
        }

        item.MaLoaiPhuongTien = model.MaLoaiPhuongTien;
        item.BienSoXe = model.BienSoXe;
        item.NhanHieu = model.NhanHieu;
        item.MauSac = model.MauSac;
        item.MoTa = model.MoTa;
        item.TrangThai = model.TrangThai;
        await _db.SaveChangesAsync();

        TempData["Msg"] = "Cập nhật phương tiện thành công.";
        return RedirectToAction(nameof(Index));
    }

    // ===== XÓA =====
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.PhuongTiens.AsNoTracking()
            .Include(p => p.LoaiPhuongTien).Include(p => p.ChuPhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhuongTien == id);
        if (item == null) return NotFound();

        var chu = LaAdmin ? null : await ChuHienTai();
        return CoQuyen(item, chu) ? View(item) : TuChoi();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.PhuongTiens.FindAsync(id);
        if (item == null) return NotFound();

        var chu = LaAdmin ? null : await ChuHienTai();
        if (!CoQuyen(item, chu)) return TuChoi();

        if (await CoPhieuHoatDong(id))
        {
            TempData["Err"] = "Không thể xóa: phương tiện đang có lượt gửi xe hoạt động.";
            return RedirectToAction(nameof(Index));
        }
        if (await _db.PhieuGuiXes.AnyAsync(p => p.MaPhuongTien == id))
        {
            TempData["Err"] = "Không thể xóa: phương tiện đã có lịch sử gửi xe. Hãy chuyển sang Ngừng hoạt động.";
            return RedirectToAction(nameof(Index));
        }

        _db.PhuongTiens.Remove(item);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Đã xóa phương tiện.";
        return RedirectToAction(nameof(Index));
    }

    // ===== HÀM DÙNG CHUNG =====
    private Task<bool> CoPhieuHoatDong(int maPhuongTien) =>
        _db.PhieuGuiXes.AnyAsync(p => p.MaPhuongTien == maPhuongTien &&
            (p.TrangThai == TrangThaiPhieu.ChoXacNhan || p.TrangThai == TrangThaiPhieu.DangGui));

    private async Task KiemTraHopLe(PhuongTien model)
    {
        if (!string.IsNullOrEmpty(model.BienSoXe) &&
            await _db.PhuongTiens.AnyAsync(p => p.BienSoXe == model.BienSoXe && p.MaPhuongTien != model.MaPhuongTien))
            ModelState.AddModelError(nameof(model.BienSoXe), "Biển số xe đã tồn tại.");

        if (model.MaLoaiPhuongTien > 0 &&
            !await _db.LoaiPhuongTiens.AnyAsync(l => l.MaLoaiPhuongTien == model.MaLoaiPhuongTien))
            ModelState.AddModelError(nameof(model.MaLoaiPhuongTien), "Loại phương tiện không tồn tại.");
    }

    private async Task NapDanhSach(int? maLoai = null, int? maChu = null)
    {
        var loais = await _db.LoaiPhuongTiens
            .Where(l => l.TrangThai || l.MaLoaiPhuongTien == maLoai)
            .OrderBy(l => l.TenLoaiPhuongTien).ToListAsync();
        ViewBag.LoaiList = new SelectList(loais, "MaLoaiPhuongTien", "TenLoaiPhuongTien", maLoai);
        ViewBag.LaAdmin = LaAdmin;

        if (LaAdmin)
        {
            var chus = await _db.ChuPhuongTiens.Where(c => c.TrangThai)
                .OrderBy(c => c.HoTen).ToListAsync();
            ViewBag.ChuList = new SelectList(chus, "MaChuPhuongTien", "HoTen", maChu);
        }
    }
}