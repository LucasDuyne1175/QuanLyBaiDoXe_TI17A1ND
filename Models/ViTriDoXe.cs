// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity vị trí đỗ xe
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public static class TrangThaiViTri
{
    public const string ConTrong = "Còn trống";
    public const string DangSuDung = "Đang sử dụng";
    public const string TamNgung = "Tạm ngừng";

    public static readonly string[] All = { ConTrong, DangSuDung, TamNgung };
}

public class ViTriDoXe
{
    [Key] public int MaViTri { get; set; }

    [Required(ErrorMessage = "Tên vị trí là bắt buộc")]
    [StringLength(50)]
    [Display(Name = "Tên vị trí")]
    public string TenViTri { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại phương tiện")]
    [Display(Name = "Loại phương tiện")]
    public int MaLoaiPhuongTien { get; set; }

    [Required(ErrorMessage = "Khu vực là bắt buộc")]
    [StringLength(50)]
    [Display(Name = "Khu vực")]
    public string KhuVuc { get; set; } = "";

    [Range(1, 10, ErrorMessage = "Tầng phải từ 1 đến 10")]
    [Display(Name = "Tầng")]
    public int Tang { get; set; } = 1;

    [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = TrangThaiViTri.ConTrong;

    [StringLength(500)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    public LoaiPhuongTien? LoaiPhuongTien { get; set; }
}