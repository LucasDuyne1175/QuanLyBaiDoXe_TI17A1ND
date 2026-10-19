// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: ViewModel danh sách phiếu (tìm kiếm, lọc, phân trang) và bộ phân trang dùng chung
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.ViewModels;

public class QuanLyPhieuListViewModel
{
    public List<PhieuGuiXe> Items { get; set; } = new();

    public string? BienSo { get; set; }
    public string? TenChu { get; set; }
    public int? MaLoai { get; set; }
    public string? TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }

    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalItems { get; set; }

    public SelectList LoaiList { get; set; } = null!;
    public SelectList TrangThaiList { get; set; } = null!;
}

public class PagerViewModel
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public Func<int, Dictionary<string, string>> Route { get; set; } = _ => new();
}