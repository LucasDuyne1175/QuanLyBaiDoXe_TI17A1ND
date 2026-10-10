// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity loại phương tiện
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public class LoaiPhuongTien
{
    [Key] public int MaLoaiPhuongTien { get; set; }

    [Required(ErrorMessage = "Tên loại phương tiện là bắt buộc")]
    [StringLength(100)]
    [Display(Name = "Tên loại phương tiện")]
    public string TenLoaiPhuongTien { get; set; } = "";

    [Range(typeof(decimal), "0.01", "100000000", ErrorMessage = "Đơn giá phải lớn hơn 0")]
    [Display(Name = "Đơn giá theo giờ")]
    public decimal DonGiaTheoGio { get; set; }

    [StringLength(500)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Display(Name = "Hoạt động")]
    public bool TrangThai { get; set; } = true;
}