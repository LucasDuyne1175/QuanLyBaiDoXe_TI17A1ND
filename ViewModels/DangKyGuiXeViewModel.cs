// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: ViewModel đăng ký gửi xe
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.ViewModels;

public class DangKyGuiXeViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phương tiện")]
    [Display(Name = "Phương tiện")]
    public int MaPhuongTien { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vị trí đỗ")]
    [Display(Name = "Vị trí đỗ")]
    public int MaViTri { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thời gian dự kiến vào")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Thời gian dự kiến vào")]
    public DateTime? ThoiGianDuKienVao { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thời gian dự kiến ra")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Thời gian dự kiến ra")]
    public DateTime? ThoiGianDuKienRa { get; set; }
}