// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Mã hóa mật khẩu SHA256
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBaiDoXe_TI17A1ND.Helpers;

public static class PasswordHelper
{
    public static string Hash(string input) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}