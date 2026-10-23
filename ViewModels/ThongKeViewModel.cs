// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: ViewModel thống kê
namespace QuanLyBaiDoXe_TI17A1ND.ViewModels;

public class DongThongKe
{
    public string Ten { get; set; } = "";
    public int SoLuong { get; set; }
}

public class DongThang
{
    public string Thang { get; set; } = "";
    public int SoLuot { get; set; }
    public decimal DoanhThu { get; set; }
}

public class BangThongKeViewModel
{
    public string TieuDe { get; set; } = "";
    public string CotTen { get; set; } = "";
    public string CotSo { get; set; } = "Số lượng";
    public List<DongThongKe> Dong { get; set; } = new();
}

public class ThongKeViewModel
{
    public List<DongThongKe> PhuongTienTheoLoai { get; set; } = new();
    public List<DongThongKe> ViTriTheoTrangThai { get; set; } = new();
    public List<DongThongKe> PhieuTheoTrangThai { get; set; } = new();
    public List<DongThongKe> LuotTheoLoai { get; set; } = new();
    public List<DongThongKe> LuotTheoViTri { get; set; } = new();
    public List<DongThongKe> LuotTheoChuXe { get; set; } = new();
    public List<DongThang> TheoThang { get; set; } = new();
    public DongThongKe? ViTriNhieuNhat { get; set; }
    public decimal TongDoanhThu { get; set; }
}