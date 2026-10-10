// Họ và tên: Đào Anh Duy
// Mã sinh viên: 23203100083
// Nội dung thực hiện: Filter kiểm tra đăng nhập và phân quyền tại server
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuanLyBaiDoXe_TI17A1ND.Filters;

public class AuthorizeRoleAttribute : ActionFilterAttribute
{
    private readonly string[] _roles;
    public AuthorizeRoleAttribute(params string[] roles) => _roles = roles;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = context.HttpContext.Session.GetString("VaiTro");

        if (string.IsNullOrEmpty(role))
        {
            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            context.Result = new RedirectToActionResult("Login", "TaiKhoan", new { returnUrl });
            return;
        }

        if (_roles.Length > 0 && !_roles.Contains(role))
            context.Result = new RedirectToActionResult("AccessDenied", "TaiKhoan", null);
    }
}