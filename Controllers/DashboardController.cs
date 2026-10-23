// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Dashboard tổng quan
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole("Admin")]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var vm = new DashboardViewModel
        {
            TongLoaiPhuongTien = await _db.LoaiPhuongTiens.CountAsync(),
            TongViTri = await _db.ViTriDoXes.CountAsync(),
            TongChuPhuongTien = await _db.ChuPhuongTiens.CountAsync(),
            TongPhuongTien = await _db.PhuongTiens.CountAsync(),
            TongPhieu = await _db.PhieuGuiXes.CountAsync(),
            PhieuChoXacNhan = await _db.PhieuGuiXes.CountAsync(p => p.TrangThai == TrangThaiPhieu.ChoXacNhan),
            XeDangGui = await _db.PhieuGuiXes.CountAsync(p => p.TrangThai == TrangThaiPhieu.DangGui),
            ViTriConTrong = await _db.ViTriDoXes.CountAsync(v => v.TrangThai == TrangThaiViTri.ConTrong),
            ViTriDangSuDung = await _db.ViTriDoXes.CountAsync(v => v.TrangThai == TrangThaiViTri.DangSuDung),
            ViTriTamNgung = await _db.ViTriDoXes.CountAsync(v => v.TrangThai == TrangThaiViTri.TamNgung)
        };
        return View(vm);
    }
}