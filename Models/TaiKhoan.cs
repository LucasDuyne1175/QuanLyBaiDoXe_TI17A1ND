// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity tài khoản
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public class TaiKhoan
{
    [Key] public int MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    [StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = "";

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(100)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = "";

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = "";

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; } = "KhachHang"; // "Admin" | "KhachHang"

    [Display(Name = "Hoạt động")]
    public bool TrangThai { get; set; } = true;
}