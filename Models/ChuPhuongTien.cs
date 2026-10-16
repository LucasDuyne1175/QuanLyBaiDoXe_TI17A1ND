// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity chủ phương tiện
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public class ChuPhuongTien
{
    [Key] public int MaChuPhuongTien { get; set; }
    public int MaTaiKhoan { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = "";

    [Display(Name = "Ngày sinh")]
    public DateTime? NgaySinh { get; set; }

    [StringLength(10)]
    [Display(Name = "Giới tính")]
    public string? GioiTinh { get; set; }

    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Display(Name = "Ngày đăng ký")]
    public DateTime NgayDangKy { get; set; }

    [Display(Name = "Hoạt động")]
    public bool TrangThai { get; set; } = true;

    public TaiKhoan? TaiKhoan { get; set; }
    public ICollection<PhuongTien> PhuongTiens { get; set; } = new List<PhuongTien>();
}