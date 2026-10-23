// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Đăng nhập, đăng xuất, đăng ký, Session
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Helpers;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

public class TaiKhoanController : Controller
{
    private readonly AppDbContext _db;
    public TaiKhoanController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(vm);

        var tk = await _db.TaiKhoans.FirstOrDefaultAsync(t => t.TenDangNhap == vm.TenDangNhap);
        if (tk == null)
        {
            ModelState.AddModelError("", "Tài khoản không tồn tại.");
            return View(vm);
        }
        if (tk.MatKhau != PasswordHelper.Hash(vm.MatKhau))
        {
            ModelState.AddModelError("", "Sai mật khẩu.");
            return View(vm);
        }
        if (!tk.TrangThai)
        {
            ModelState.AddModelError("", "Tài khoản đã bị khóa.");
            return View(vm);
        }

        HttpContext.Session.SetInt32("MaTaiKhoan", tk.MaTaiKhoan);
        HttpContext.Session.SetString("HoTen", tk.HoTen);
        HttpContext.Session.SetString("VaiTro", tk.VaiTro);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return tk.VaiTro == "Admin"
    ? RedirectToAction("Index", "Dashboard")
    : RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == vm.TenDangNhap))
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        if (!ModelState.IsValid) return View(vm);

        var tk = new TaiKhoan
        {
            TenDangNhap = vm.TenDangNhap,
            MatKhau = PasswordHelper.Hash(vm.MatKhau),
            HoTen = vm.HoTen,
            Email = vm.Email,
            VaiTro = "KhachHang",
            TrangThai = true
        };
        _db.TaiKhoans.Add(tk);
        _db.ChuPhuongTiens.Add(new ChuPhuongTien
        {
            TaiKhoan = tk,
            HoTen = vm.HoTen,
            Email = vm.Email,
            NgayDangKy = DateTime.Now,
            TrangThai = true
        });
        await _db.SaveChangesAsync();

        TempData["Msg"] = "Đăng ký thành công, hãy đăng nhập.";
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();
}