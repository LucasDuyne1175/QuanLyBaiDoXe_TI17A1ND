// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: ViewModel danh sách vị trí đỗ (tìm kiếm, lọc, sắp xếp, phân trang)
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyBaiDoXe_TI17A1ND.Models;

namespace QuanLyBaiDoXe_TI17A1ND.ViewModels;

public class ViTriListViewModel
{
    public List<ViTriDoXe> Items { get; set; } = new();

    public string? Keyword { get; set; }
    public int? MaLoai { get; set; }
    public string? KhuVuc { get; set; }
    public int? Tang { get; set; }
    public string? TrangThai { get; set; }
    public string Sort { get; set; } = "ten_asc";

    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalItems { get; set; }

    public SelectList LoaiList { get; set; } = null!;
    public SelectList KhuVucList { get; set; } = null!;
    public SelectList TangList { get; set; } = null!;
    public SelectList TrangThaiList { get; set; } = null!;
}