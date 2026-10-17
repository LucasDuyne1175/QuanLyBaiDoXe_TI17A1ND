// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Lấy hoặc tạo hồ sơ chủ phương tiện theo tài khoản
using Microsoft.EntityFrameworkCore;
using QuanLyBaiDoXe_TI17A1ND.Data;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.Helpers;

public static class ChuXeExtensions
{
    public static async Task<ChuPhuongTien?> LayHoacTaoChuXe(this AppDbContext db, int maTaiKhoan)
    {
        var chu = await db.ChuPhuongTiens.FirstOrDefaultAsync(c => c.MaTaiKhoan == maTaiKhoan);
        if (chu != null) return chu;

        var tk = await db.TaiKhoans.FindAsync(maTaiKhoan);
        if (tk == null) return null;

        chu = new ChuPhuongTien
        {
            MaTaiKhoan = tk.MaTaiKhoan,
            HoTen = tk.HoTen,
            Email = tk.Email,
            NgayDangKy = DateTime.Now,
            TrangThai = true
        };
        db.ChuPhuongTiens.Add(chu);
        await db.SaveChangesAsync();
        return chu;
    }
}