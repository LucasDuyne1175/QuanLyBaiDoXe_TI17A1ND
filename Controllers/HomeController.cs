// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Trang chủ, số vị trí trống và bảng giá
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.ConTrong = await _db.ViTriDoXes.CountAsync(v => v.TrangThai == TrangThaiViTri.ConTrong);
        ViewBag.TongViTri = await _db.ViTriDoXes.CountAsync();
        ViewBag.BangGia = await _db.LoaiPhuongTiens.AsNoTracking()
            .Where(l => l.TrangThai).OrderBy(l => l.DonGiaTheoGio).ToListAsync();
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}