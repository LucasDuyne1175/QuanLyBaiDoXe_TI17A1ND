// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Entity phiếu gửi xe
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiDoXe_TI17A1ND.Models;

public static class TrangThaiPhieu
{
    public const string ChoXacNhan = "Chờ xác nhận";
    public const string DangGui = "Đang gửi";
    public const string DaLayXe = "Đã lấy xe";
    public const string DaHuy = "Đã hủy";

    public static readonly string[] All = { ChoXacNhan, DangGui, DaLayXe, DaHuy };
}

public class PhieuGuiXe
{
    [Key] public int MaPhieu { get; set; }
    public int MaPhuongTien { get; set; }
    public int MaViTri { get; set; }

    [Display(Name = "Ngày đăng ký")]
    public DateTime NgayDangKy { get; set; }

    [Display(Name = "Dự kiến vào")]
    public DateTime ThoiGianDuKienVao { get; set; }

    [Display(Name = "Dự kiến ra")]
    public DateTime ThoiGianDuKienRa { get; set; }

    [Display(Name = "Vào thực tế")]
    public DateTime? ThoiGianVaoThucTe { get; set; }

    [Display(Name = "Ra thực tế")]
    public DateTime? ThoiGianRaThucTe { get; set; }

    [Display(Name = "Đơn giá theo giờ")]
    public decimal DonGiaTheoGio { get; set; }

    [Display(Name = "Thành tiền")]
    public decimal? ThanhTien { get; set; }

    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = TrangThaiPhieu.ChoXacNhan;

    public PhuongTien? PhuongTien { get; set; }
    public ViTriDoXe? ViTriDoXe { get; set; }
}