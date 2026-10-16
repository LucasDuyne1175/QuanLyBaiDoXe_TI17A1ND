// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity phương tiện
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public class PhuongTien
{
    [Key] public int MaPhuongTien { get; set; }
    public int MaChuPhuongTien { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại phương tiện")]
    [Display(Name = "Loại phương tiện")]
    public int MaLoaiPhuongTien { get; set; }

    [Required(ErrorMessage = "Biển số xe là bắt buộc")]
    [StringLength(20)]
    [Display(Name = "Biển số xe")]
    public string BienSoXe { get; set; } = "";

    [StringLength(50)]
    [Display(Name = "Nhãn hiệu")]
    public string? NhanHieu { get; set; }

    [StringLength(30)]
    [Display(Name = "Màu sắc")]
    public string? MauSac { get; set; }

    [StringLength(500)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Display(Name = "Hoạt động")]
    public bool TrangThai { get; set; } = true;

    public ChuPhuongTien? ChuPhuongTien { get; set; }
    public LoaiPhuongTien? LoaiPhuongTien { get; set; }
}