// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quản lý phiếu gửi xe, xác nhận xe vào/ra, tính phí, hủy phiếu
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Filters;
using QuanLyBaiDoXe_TI17A1ND.Helpers;
using QuanLyBaiDoXe_TI17A1ND.Models;
using QuanLyBaiDoXe_TI17A1ND.ViewModels;

namespace QuanLyBaiDoXe_TI17A1ND.Controllers;

[AuthorizeRole("Admin")]
public class QuanLyPhieuController : Controller
{
    private const int PageSize = 10;
    private readonly AppDbContext _db;
    public QuanLyPhieuController(AppDbContext db) => _db = db;

    // Truy vấn phiếu kèm đầy đủ thông tin liên quan
    private IQueryable<PhieuGuiXe> PhieuDayDu() => _db.PhieuGuiXes
        .Include(p => p.PhuongTien).ThenInclude(x => x!.ChuPhuongTien)
        .Include(p => p.PhuongTien).ThenInclude(x => x!.LoaiPhuongTien)
        .Include(p => p.ViTriDoXe);

    // ================= DANH SÁCH =================
    public async Task<IActionResult> Index(string? bienSo, string? tenChu, int? maLoai,
        string? trangThai, DateTime? tuNgay, DateTime? denNgay, int page = 1)
    {
        var q = PhieuDayDu().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(bienSo))
        {
            bienSo = bienSo.Trim();
            q = q.Where(p => p.PhuongTien!.BienSoXe.Contains(bienSo));
        }
        if (!string.IsNullOrWhiteSpace(tenChu))
        {
            tenChu = tenChu.Trim();
            q = q.Where(p => p.PhuongTien!.ChuPhuongTien!.HoTen.Contains(tenChu));
        }
        if (maLoai.HasValue) q = q.Where(p => p.PhuongTien!.MaLoaiPhuongTien == maLoai.Value);
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(p => p.TrangThai == trangThai);
        if (tuNgay.HasValue) q = q.Where(p => p.NgayDangKy >= tuNgay.Value.Date);
        if (denNgay.HasValue)
        {
            var den = denNgay.Value.Date.AddDays(1);
            q = q.Where(p => p.NgayDangKy < den);
        }

        var total = await q.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = await q.OrderByDescending(p => p.NgayDangKy).ThenByDescending(p => p.MaPhieu)
            .Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

        var loais = await _db.LoaiPhuongTiens.OrderBy(l => l.TenLoaiPhuongTien).ToListAsync();

        return View(new QuanLyPhieuListViewModel
        {
            Items = items,
            BienSo = bienSo,
            TenChu = tenChu,
            MaLoai = maLoai,
            TrangThai = trangThai,
            TuNgay = tuNgay,
            DenNgay = denNgay,
            Page = page,
            TotalPages = totalPages,
            TotalItems = total,
            LoaiList = new SelectList(loais, "MaLoaiPhuongTien", "TenLoaiPhuongTien", maLoai),
            TrangThaiList = new SelectList(TrangThaiPhieu.All, trangThai)
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var phieu = await PhieuDayDu().AsNoTracking().FirstOrDefaultAsync(p => p.MaPhieu == id);
        return phieu == null ? NotFound() : View(phieu);
    }

    // ================= XÁC NHẬN XE VÀO =================
    [HttpGet]
    public async Task<IActionResult> XacNhanVao(int id)
    {
        var phieu = await PhieuDayDu().AsNoTracking().FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();

        if (phieu.TrangThai != TrangThaiPhieu.ChoXacNhan)
        {
            TempData["Err"] = $"Phiếu #{id} đang ở trạng thái \"{phieu.TrangThai}\", không thể xác nhận xe vào.";
            return RedirectToAction(nameof(Details), new { id });
        }
        return View(phieu);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhanVao(int id, string? bienSoThucTe)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var phieu = await PhieuDayDu().FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();

        var loi = await KiemTraXeVao(phieu, bienSoThucTe);
        if (loi != null)
        {
            TempData["Err"] = loi;
            return RedirectToAction(nameof(XacNhanVao), new { id });
        }

        phieu.ThoiGianVaoThucTe = DateTime.Now;
        phieu.TrangThai = TrangThaiPhieu.DangGui;
        phieu.ViTriDoXe!.TrangThai = TrangThaiViTri.DangSuDung;

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        TempData["Msg"] = $"Đã xác nhận xe {phieu.PhuongTien!.BienSoXe} vào bãi, vị trí {phieu.ViTriDoXe.TenViTri}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // Trả về thông báo lỗi nếu không hợp lệ, null nếu hợp lệ
    private async Task<string?> KiemTraXeVao(PhieuGuiXe phieu, string? bienSoThucTe)
    {
        var pt = phieu.PhuongTien;
        var vt = phieu.ViTriDoXe;

        // 1. Phiếu còn hợp lệ
        if (phieu.TrangThai != TrangThaiPhieu.ChoXacNhan)
            return $"Phiếu đang ở trạng thái \"{phieu.TrangThai}\", không thể xác nhận xe vào.";
        if (DateTime.Now > phieu.ThoiGianDuKienRa)
            return "Phiếu đã quá thời gian dự kiến ra. Vui lòng hủy phiếu và đăng ký lại.";

        // 2. Phương tiện đúng với phiếu
        if (pt == null || vt == null) return "Dữ liệu phiếu không đầy đủ.";
        if (!string.Equals((bienSoThucTe ?? "").Trim(), pt.BienSoXe, StringComparison.OrdinalIgnoreCase))
            return $"Biển số thực tế không khớp với phiếu (phiếu đăng ký biển số {pt.BienSoXe}).";
        if (!pt.TrangThai) return "Phương tiện đang ngừng hoạt động.";
        if (pt.ChuPhuongTien == null || !pt.ChuPhuongTien.TrangThai) return "Chủ phương tiện đang ngừng hoạt động.";

        // 3. Vị trí đỗ còn phù hợp
        if (vt.TrangThai == TrangThaiViTri.TamNgung) return "Vị trí đỗ đang tạm ngừng hoạt động.";
        if (vt.MaLoaiPhuongTien != pt.MaLoaiPhuongTien) return "Vị trí đỗ không phù hợp loại phương tiện.";

        // 4. Không có lượt gửi khác đang chiếm vị trí
        bool dangCoXe = vt.TrangThai == TrangThaiViTri.DangSuDung ||
            await _db.PhieuGuiXes.AnyAsync(p => p.MaViTri == vt.MaViTri &&
                p.TrangThai == TrangThaiPhieu.DangGui && p.MaPhieu != phieu.MaPhieu);
        if (dangCoXe) return $"Vị trí {vt.TenViTri} đang có xe khác gửi.";

        if (await _db.ViTriBiChiemAsync(vt.MaViTri, phieu.ThoiGianDuKienVao, phieu.ThoiGianDuKienRa, phieu.MaPhieu))
            return $"Vị trí {vt.TenViTri} đang bị phiếu hợp lệ khác chiếm trong khoảng thời gian này.";

        // 5. Phương tiện không có lượt gửi khác đang hoạt động
        if (await _db.PhieuGuiXes.ConHieuLuc().AnyAsync(p => p.MaPhuongTien == pt.MaPhuongTien && p.MaPhieu != phieu.MaPhieu))
            return "Phương tiện đang có lượt gửi xe khác còn hiệu lực.";

        return null;
    }

    // ================= XÁC NHẬN XE RA + TÍNH PHÍ =================
    [HttpGet]
    public async Task<IActionResult> XacNhanRa(int id)
    {
        var phieu = await PhieuDayDu().AsNoTracking().FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();

        if (phieu.TrangThai != TrangThaiPhieu.DangGui || phieu.ThoiGianVaoThucTe == null)
        {
            TempData["Err"] = $"Phiếu #{id} đang ở trạng thái \"{phieu.TrangThai}\", không thể xác nhận xe ra.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Tạm tính nếu xe ra ngay bây giờ (chỉ để hiển thị)
        var bayGio = DateTime.Now;
        ViewBag.BayGio = bayGio;
        ViewBag.SoGio = TinhPhiHelper.SoGioTinhPhi(phieu.ThoiGianVaoThucTe.Value, bayGio);
        ViewBag.TamTinh = TinhPhiHelper.ThanhTien((int)ViewBag.SoGio, phieu.DonGiaTheoGio);
        return View(phieu);
    }

    [HttpPost, ActionName("XacNhanRa"), ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhanRaConfirmed(int id)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var phieu = await PhieuDayDu().FirstOrDefaultAsync(p => p.MaPhieu == id);
        if (phieu == null) return NotFound();

        if (phieu.TrangThai != TrangThaiPhieu.DangGui || phieu.ThoiGianVaoThucTe == null)
        {
            TempData["Err"] = "Chỉ xác nhận xe ra cho phiếu đang ở trạng thái Đang gửi.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var ra = DateTime.Now;
        if (ra <= phieu.ThoiGianVaoThucTe.Value)
        {
            TempData["Err"] = "Thời gian ra thực tế phải sau thời gian vào thực tế.";
            return RedirectToAction(nameof(XacNhanRa), new { id });
        }

        // Hệ thống tự tính: người dùng không nhập số giờ hay thành tiền
        int soGio = TinhPhiHelper.SoGioTinhPhi(phieu.ThoiGianVaoThucTe.Value, ra);
        phieu.ThoiGianRaThucTe = ra;
        phieu.ThanhTien = TinhPhiHelper.ThanhTien(soGio, phieu.DonGiaTheoGio);
        phieu.TrangThai = TrangThaiPhieu.DaLayXe;

        // Trả vị trí về Còn trống nếu không còn xe nào khác đang chiếm
        var vt = phieu.ViTriDoXe!;
        bool conXeKhac = await _db.PhieuGuiXes.AnyAsync(p => p.MaViTri == vt.MaViTri &&
            p.TrangThai == TrangThaiPhieu.DangGui && p.MaPhieu != phieu.MaPhieu);
        if (!conXeKhac && vt.TrangThai == TrangThaiViTri.DangSuDung)
            vt.TrangThai = TrangThaiViTri.ConTrong;

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        TempData["Msg"] = $"Xe {phieu.PhuongTien!.BienSoXe} đã ra bãi. Gửi {soGio} giờ, thành tiền {phieu.ThanhTien:N0} đ.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // ================= HỦY PHIẾU =================
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Huy(int id)
    {
        var phieu = await _db.PhieuGuiXes.FindAsync(id);
        if (phieu == null) return NotFound();

        // Chỉ hủy phiếu Chờ xác nhận. Xe đã vào bãi thì phải xác nhận xe ra.
        if (phieu.TrangThai != TrangThaiPhieu.ChoXacNhan)
        {
            TempData["Err"] = $"Không thể hủy phiếu đang ở trạng thái \"{phieu.TrangThai}\".";
            return RedirectToAction(nameof(Details), new { id });
        }

        phieu.TrangThai = TrangThaiPhieu.DaHuy;
        await _db.SaveChangesAsync();
        TempData["Msg"] = $"Đã hủy phiếu #{id}.";
        return RedirectToAction(nameof(Index));
    }
}