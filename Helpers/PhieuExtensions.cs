// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Truy vấn LINQ kiểm tra vị trí bị chiếm và xe đang gửi
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Helpers;

public static class PhieuExtensions
{
    // Phiếu còn hiệu lực = Chờ xác nhận hoặc Đang gửi (Đã hủy / Đã lấy xe không tính)
    public static IQueryable<PhieuGuiXe> ConHieuLuc(this IQueryable<PhieuGuiXe> q) =>
        q.Where(p => p.TrangThai == TrangThaiPhieu.ChoXacNhan || p.TrangThai == TrangThaiPhieu.DangGui);

    // Vị trí có bị phiếu khác chiếm trong khoảng [vao, ra) hay không
    public static Task<bool> ViTriBiChiemAsync(this AppDbContext db, int maViTri,
        DateTime vao, DateTime ra, int? boQuaMaPhieu = null) =>
        db.PhieuGuiXes.ConHieuLuc().AnyAsync(p =>
            p.MaViTri == maViTri &&
            p.ThoiGianDuKienVao < ra &&
            p.ThoiGianDuKienRa > vao &&
            (boQuaMaPhieu == null || p.MaPhieu != boQuaMaPhieu));

    // Phương tiện đang có lượt gửi hoạt động hay không
    public static Task<bool> PhuongTienDangGuiAsync(this AppDbContext db, int maPhuongTien) =>
        db.PhieuGuiXes.ConHieuLuc().AnyAsync(p => p.MaPhuongTien == maPhuongTien);
}