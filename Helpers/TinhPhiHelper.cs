// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Quy tắc làm tròn giờ và tính phí gửi xe
namespace QuanLyBaiDoXe_TI17A1ND.Helpers;

public static class TinhPhiHelper
{
    // Quy tắc: làm tròn LÊN theo giờ, tối thiểu 1 giờ, bỏ qua giây lẻ
    public static int SoGioTinhPhi(DateTime vao, DateTime ra)
    {
        var phut = Math.Floor((ra - vao).TotalMinutes);
        return Math.Max(1, (int)Math.Ceiling(phut / 60.0));
    }

    public static decimal ThanhTien(int soGio, decimal donGiaTheoGio) => soGio * donGiaTheoGio;
}