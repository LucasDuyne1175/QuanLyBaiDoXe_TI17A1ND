// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: ViewModel hồ sơ cá nhân của khách hàng
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.ViewModels;

public class HoSoViewModel
{
    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ngày sinh")]
    public DateTime? NgaySinh { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giới tính")]
    [Display(Name = "Giới tính")]
    public string? GioiTinh { get; set; }

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [RegularExpression(@"^(0|\+84)[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ (VD: 0901234567)")]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    [StringLength(200)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }
}