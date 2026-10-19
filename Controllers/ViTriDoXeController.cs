// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quản lý và tra cứu vị trí đỗ xe
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole] // chỉ cần đăng nhập: Admin và Khách đều xem/tra cứu được
public class ViTriDoXeController : Controller
{
    private const int PageSize = 8;
    private readonly AppDbContext _db;
    public ViTriDoXeController(AppDbContext db) => _db = db;

    // ========== DANH SÁCH: Truy vấn -> Tìm kiếm -> Lọc -> Sắp xếp -> Phân trang ==========
    public async Task<IActionResult> Index(string? keyword, int? maLoai, string? khuVuc,
        int? tang, string? trangThai, string? sort, int page = 1)
    {
        var q = _db.ViTriDoXes.Include(v => v.LoaiPhuongTien).AsNoTracking().AsQueryable();

        // Tìm kiếm theo tên vị trí hoặc khu vực
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            q = q.Where(v => v.TenViTri.Contains(keyword) || v.KhuVuc.Contains(keyword));
        }

        // Lọc (các điều kiện kết hợp được với nhau)
        if (maLoai.HasValue) q = q.Where(v => v.MaLoaiPhuongTien == maLoai.Value);
        if (!string.IsNullOrEmpty(khuVuc)) q = q.Where(v => v.KhuVuc == khuVuc);
        if (tang.HasValue) q = q.Where(v => v.Tang == tang.Value);
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(v => v.TrangThai == trangThai);

        // Sắp xếp
        sort = string.IsNullOrEmpty(sort) ? "ten_asc" : sort;
        q = sort switch
        {
            "ten_desc" => q.OrderByDescending(v => v.TenViTri),
            "tang_asc" => q.OrderBy(v => v.Tang).ThenBy(v => v.TenViTri),
            "tang_desc" => q.OrderByDescending(v => v.Tang).ThenBy(v => v.TenViTri),
            _ => q.OrderBy(v => v.TenViTri)
        };

        // Phân trang bằng Skip/Take trên truy vấn EF Core
        var totalItems = await q.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = await q.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

        var loais = await _db.LoaiPhuongTiens.OrderBy(l => l.TenLoaiPhuongTien).ToListAsync();
        var khuVucs = await _db.ViTriDoXes.Select(v => v.KhuVuc).Distinct().OrderBy(k => k).ToListAsync();
        var tangs = await _db.ViTriDoXes.Select(v => v.Tang).Distinct().OrderBy(t => t).ToListAsync();

        var vm = new ViTriListViewModel
        {
            Items = items,
            Keyword = keyword,
            MaLoai = maLoai,
            KhuVuc = khuVuc,
            Tang = tang,
            TrangThai = trangThai,
            Sort = sort,
            Page = page,
            TotalPages = totalPages,
            TotalItems = totalItems,
            LoaiList = new SelectList(loais, "MaLoaiPhuongTien", "TenLoaiPhuongTien", maLoai),
            KhuVucList = new SelectList(khuVucs, khuVuc),
            TangList = new SelectList(tangs, tang),
            TrangThaiList = new SelectList(TrangThaiViTri.All, trangThai)
        };
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.ViTriDoXes.Include(v => v.LoaiPhuongTien)
            .AsNoTracking().FirstOrDefaultAsync(v => v.MaViTri == id);
        return item == null ? NotFound() : View(item);
    }

    // ========== CREATE ==========
    [AuthorizeRole("Admin")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapDanhSach();
        return View(new ViTriDoXe());
    }

    [AuthorizeRole("Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ViTriDoXe model)
    {
        await KiemTraHopLe(model);
        if (!ModelState.IsValid)
        {
            await NapDanhSach(model.MaLoaiPhuongTien);
            return View(model);
        }

        _db.ViTriDoXes.Add(model);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Thêm vị trí đỗ xe thành công.";
        return RedirectToAction(nameof(Index));
    }

    // ========== EDIT ==========
    [AuthorizeRole("Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.ViTriDoXes.FindAsync(id);
        if (item == null) return NotFound();
        await NapDanhSach(item.MaLoaiPhuongTien);
        return View(item);
    }

    [AuthorizeRole("Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ViTriDoXe model)
    {
        if (id != model.MaViTri) return BadRequest();

        await KiemTraHopLe(model);
        if (!ModelState.IsValid)
        {
            await NapDanhSach(model.MaLoaiPhuongTien);
            return View(model);
        }

        _db.ViTriDoXes.Update(model);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Cập nhật vị trí đỗ xe thành công.";
        return RedirectToAction(nameof(Index));
    }

    // ========== DELETE ==========
    [AuthorizeRole("Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.ViTriDoXes.Include(v => v.LoaiPhuongTien)
            .AsNoTracking().FirstOrDefaultAsync(v => v.MaViTri == id);
        return item == null ? NotFound() : View(item);
    }

    [AuthorizeRole("Admin")]
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.ViTriDoXes.FindAsync(id);
        if (item == null) return NotFound();

        if (await _db.PhieuGuiXes.AnyAsync(p => p.MaViTri == id))
        {
            TempData["Err"] = "Không thể xóa: vị trí đã có phiếu gửi xe.";
            return RedirectToAction(nameof(Index));
        }

        _db.ViTriDoXes.Remove(item);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Đã xóa vị trí đỗ xe.";
        return RedirectToAction(nameof(Index));
    }

    // ========== HÀM DÙNG CHUNG ==========
    private async Task NapDanhSach(int? maLoaiChon = null)
    {
        var loais = await _db.LoaiPhuongTiens.Where(l => l.TrangThai)
            .OrderBy(l => l.TenLoaiPhuongTien).ToListAsync();
        ViewBag.LoaiList = new SelectList(loais, "MaLoaiPhuongTien", "TenLoaiPhuongTien", maLoaiChon);
        ViewBag.TrangThaiList = new SelectList(TrangThaiViTri.All);
    }

    private async Task KiemTraHopLe(ViTriDoXe model)
    {
        if (await _db.ViTriDoXes.AnyAsync(v =>
                v.TenViTri == model.TenViTri && v.MaViTri != model.MaViTri))
            ModelState.AddModelError(nameof(model.TenViTri), "Tên vị trí đã tồn tại.");

        if (model.MaLoaiPhuongTien > 0 &&
            !await _db.LoaiPhuongTiens.AnyAsync(l => l.MaLoaiPhuongTien == model.MaLoaiPhuongTien))
            ModelState.AddModelError(nameof(model.MaLoaiPhuongTien), "Loại phương tiện không tồn tại.");

        if (!TrangThaiViTri.All.Contains(model.TrangThai))
            ModelState.AddModelError(nameof(model.TrangThai), "Trạng thái không hợp lệ.");
    }
}