// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quản lý loại phương tiện
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole("Admin")]
public class LoaiPhuongTienController : Controller
{
    private readonly AppDbContext _db;
    public LoaiPhuongTienController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? keyword)
    {
        var q = _db.LoaiPhuongTiens.AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
            q = q.Where(l => l.TenLoaiPhuongTien.Contains(keyword));

        ViewBag.Keyword = keyword;
        return View(await q.OrderBy(l => l.TenLoaiPhuongTien).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.LoaiPhuongTiens.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new LoaiPhuongTien());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LoaiPhuongTien model)
    {
        if (await _db.LoaiPhuongTiens.AnyAsync(l => l.TenLoaiPhuongTien == model.TenLoaiPhuongTien))
            ModelState.AddModelError(nameof(model.TenLoaiPhuongTien), "Tên loại phương tiện đã tồn tại.");
        if (!ModelState.IsValid) return View(model);

        _db.LoaiPhuongTiens.Add(model);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Thêm loại phương tiện thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.LoaiPhuongTiens.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LoaiPhuongTien model)
    {
        if (id != model.MaLoaiPhuongTien) return BadRequest();

        if (await _db.LoaiPhuongTiens.AnyAsync(l =>
                l.TenLoaiPhuongTien == model.TenLoaiPhuongTien && l.MaLoaiPhuongTien != id))
            ModelState.AddModelError(nameof(model.TenLoaiPhuongTien), "Tên loại phương tiện đã tồn tại.");
        if (!ModelState.IsValid) return View(model);

        _db.LoaiPhuongTiens.Update(model);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Cập nhật loại phương tiện thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DoiTrangThai(int id)
    {
        var item = await _db.LoaiPhuongTiens.FindAsync(id);
        if (item == null) return NotFound();

        item.TrangThai = !item.TrangThai;
        await _db.SaveChangesAsync();
        TempData["Msg"] = item.TrangThai ? "Đã kích hoạt loại phương tiện." : "Đã ngừng hoạt động loại phương tiện.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.LoaiPhuongTiens.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.LoaiPhuongTiens.FindAsync(id);
        if (item == null) return NotFound();

        // Mở comment khi đã có entity PhuongTien (Module 3) và ViTriDoXe (Module 2):
        // bool dangDung = await _db.PhuongTiens.AnyAsync(p => p.MaLoaiPhuongTien == id)
        //              || await _db.ViTriDoXes.AnyAsync(v => v.MaLoaiPhuongTien == id);
        // if (dangDung)
        // {
        //     TempData["Err"] = "Không thể xóa: loại này đang được phương tiện hoặc vị trí đỗ sử dụng.";
        //     return RedirectToAction(nameof(Index));
        // }

        _db.LoaiPhuongTiens.Remove(item);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Đã xóa loại phương tiện.";
        return RedirectToAction(nameof(Index));
    }
}