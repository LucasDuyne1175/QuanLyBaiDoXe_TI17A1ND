// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Đăng ký gửi xe, kiểm tra trùng lịch, lịch sử gửi xe
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Helpers;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole]
public class PhieuGuiXeController : Controller
{
    private readonly AppDbContext _db;
    public PhieuGuiXeController(AppDbContext db) => _db = db;

    private async Task<ChuPhuongTien?> ChuHienTai()
    {
        var maTk = HttpContext.Session.GetInt32("MaTaiKhoan");
        return maTk == null ? null : await _db.LayHoacTaoChuXe(maTk.Value);
    }

    private IActionResult TuChoi() => RedirectToAction("AccessDenied", "TaiKhoan");

    // ================= ĐĂNG KÝ GỬI XE =================
    [AuthorizeRole("KhachHang")]
    [HttpGet]
    public async Task<IActionResult> DangKy(int? maViTri)
    {
        var chu = await ChuHienTai();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        await NapDanhSach(chu);
        return View(new DangKyGuiXeViewModel { MaViTri = maViTri ?? 0 });
    }

    [AuthorizeRole("KhachHang")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DangKy(DangKyGuiXeViewModel vm)
    {
        var chu = await ChuHienTai();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        // Transaction Serializable để 2 khách không giữ trùng một vị trí cùng lúc
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        // 1. Chủ phương tiện đang hoạt động
        if (!chu.TrangThai)
            ModelState.AddModelError("", "Hồ sơ chủ phương tiện của bạn đang ngừng hoạt động.");

        // 2. Phương tiện thuộc đúng khách đang đăng nhập + 3. đang hoạt động
        var pt = await _db.PhuongTiens.Include(p => p.LoaiPhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhuongTien == vm.MaPhuongTien);
        if (pt == null || pt.MaChuPhuongTien != chu.MaChuPhuongTien)
        {
            ModelState.AddModelError(nameof(vm.MaPhuongTien), "Phương tiện không thuộc tài khoản của bạn.");
            pt = null;
        }
        else if (!pt.TrangThai)
            ModelState.AddModelError(nameof(vm.MaPhuongTien), "Phương tiện đang ngừng hoạt động.");

        // 4. Vị trí tồn tại và không Tạm ngừng + 5. đúng loại phương tiện
        var vt = await _db.ViTriDoXes.FirstOrDefaultAsync(v => v.MaViTri == vm.MaViTri);
        if (vm.MaViTri > 0 && vt == null)
            ModelState.AddModelError(nameof(vm.MaViTri), "Vị trí đỗ không tồn tại.");
        else if (vt != null && vt.TrangThai == TrangThaiViTri.TamNgung)
            ModelState.AddModelError(nameof(vm.MaViTri), "Vị trí đỗ đang tạm ngừng hoạt động.");
        else if (vt != null && pt != null && vt.MaLoaiPhuongTien != pt.MaLoaiPhuongTien)
            ModelState.AddModelError(nameof(vm.MaViTri), "Vị trí đỗ không phù hợp với loại phương tiện đã chọn.");

        // 6. Thời gian hợp lệ
        if (vm.ThoiGianDuKienVao.HasValue && vm.ThoiGianDuKienRa.HasValue)
        {
            var vao = vm.ThoiGianDuKienVao.Value;
            var ra = vm.ThoiGianDuKienRa.Value;

            if (ra <= vao)
                ModelState.AddModelError(nameof(vm.ThoiGianDuKienRa), "Thời gian dự kiến ra phải sau thời gian dự kiến vào.");
            else if (vao < DateTime.Now.AddMinutes(-5))
                ModelState.AddModelError(nameof(vm.ThoiGianDuKienVao), "Thời gian dự kiến vào không được ở quá khứ.");
            else if ((ra - vao).TotalDays > 30)
                ModelState.AddModelError(nameof(vm.ThoiGianDuKienRa), "Mỗi lượt gửi tối đa 30 ngày.");
            else
            {
                // 7. Phương tiện không có hai lượt gửi hoạt động đồng thời
                if (pt != null && await _db.PhuongTienDangGuiAsync(pt.MaPhuongTien))
                    ModelState.AddModelError(nameof(vm.MaPhuongTien),
                        "Phương tiện này đã có lượt gửi xe đang hoạt động (chờ xác nhận hoặc đang gửi).");

                // 8. Vị trí không trùng khoảng thời gian với phiếu hợp lệ khác (LINQ)
                if (vt != null && await _db.ViTriBiChiemAsync(vt.MaViTri, vao, ra))
                    ModelState.AddModelError(nameof(vm.MaViTri),
                        "Vị trí này đã có phiếu gửi xe trong khoảng thời gian bạn chọn.");
            }
        }

        if (!ModelState.IsValid)
        {
            await NapDanhSach(chu);
            return View(vm);
        }

        // NgayDangKy và đơn giá do hệ thống xác định
        var phieu = new PhieuGuiXe
        {
            MaPhuongTien = pt!.MaPhuongTien,
            MaViTri = vt!.MaViTri,
            NgayDangKy = DateTime.Now,
            ThoiGianDuKienVao = vm.ThoiGianDuKienVao!.Value,
            ThoiGianDuKienRa = vm.ThoiGianDuKienRa!.Value,
            DonGiaTheoGio = pt.LoaiPhuongTien!.DonGiaTheoGio,
            TrangThai = TrangThaiPhieu.ChoXacNhan
        };
        _db.PhieuGuiXes.Add(phieu);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        TempData["Msg"] = $"Đăng ký gửi xe thành công. Mã phiếu #{phieu.MaPhieu}, đang chờ xác nhận.";
        return RedirectToAction(nameof(LichSu));
    }

    // ================= LỊCH SỬ GỬI XE =================
    [AuthorizeRole("KhachHang")]
    public async Task<IActionResult> LichSu(string? trangThai)
    {
        var chu = await ChuHienTai();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        // Chỉ lấy phiếu thuộc phương tiện của chính khách này
        var goc = _db.PhieuGuiXes.AsNoTracking()
            .Where(p => p.PhuongTien!.MaChuPhuongTien == chu.MaChuPhuongTien);

        ViewBag.Dem = await goc.GroupBy(p => p.TrangThai)
            .Select(g => new { g.Key, SoLuong = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.SoLuong);
        ViewBag.TrangThai = trangThai;

        var q = goc.Include(p => p.PhuongTien).Include(p => p.ViTriDoXe).AsQueryable();
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(p => p.TrangThai == trangThai);

        return View(await q.OrderByDescending(p => p.NgayDangKy).ThenByDescending(p => p.MaPhieu).ToListAsync());
    }

    [AuthorizeRole("KhachHang")]
    public async Task<IActionResult> ChiTiet(int id)
    {
        var chu = await ChuHienTai();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        var phieu = await _db.PhieuGuiXes.AsNoTracking()
            .Include(p => p.PhuongTien).ThenInclude(x => x!.LoaiPhuongTien)
            .Include(p => p.ViTriDoXe)
            .FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();
        if (phieu.PhuongTien!.MaChuPhuongTien != chu.MaChuPhuongTien) return TuChoi();

        return View(phieu);
    }

    // ================= HỦY PHIẾU (chỉ phiếu Chờ xác nhận) =================
    [AuthorizeRole("KhachHang")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Huy(int id)
    {
        var chu = await ChuHienTai();
        if (chu == null) return RedirectToAction("Login", "TaiKhoan");

        var phieu = await _db.PhieuGuiXes.Include(p => p.PhuongTien)
            .FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();
        if (phieu.PhuongTien!.MaChuPhuongTien != chu.MaChuPhuongTien) return TuChoi();

        if (phieu.TrangThai != TrangThaiPhieu.ChoXacNhan)
        {
            TempData["Err"] = "Chỉ được hủy phiếu đang ở trạng thái Chờ xác nhận.";
            return RedirectToAction(nameof(LichSu));
        }

        phieu.TrangThai = TrangThaiPhieu.DaHuy;
        await _db.SaveChangesAsync();
        TempData["Msg"] = $"Đã hủy phiếu #{phieu.MaPhieu}.";
        return RedirectToAction(nameof(LichSu));
    }

    // ================= HÀM DÙNG CHUNG =================
    private async Task NapDanhSach(ChuPhuongTien chu)
    {
        ViewBag.PhuongTiens = await _db.PhuongTiens.AsNoTracking()
            .Include(p => p.LoaiPhuongTien)
            .Where(p => p.MaChuPhuongTien == chu.MaChuPhuongTien && p.TrangThai)
            .OrderBy(p => p.BienSoXe).ToListAsync();

        ViewBag.ViTris = await _db.ViTriDoXes.AsNoTracking()
            .Include(v => v.LoaiPhuongTien)
            .Where(v => v.TrangThai != TrangThaiViTri.TamNgung)
            .OrderBy(v => v.TenViTri).ToListAsync();
    }
}