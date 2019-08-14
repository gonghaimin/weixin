using Microsoft.AspNetCore.Http;


namespace AuthService.Cookie
{
    public class CookieAuthenticationDefaults
    {
        /// <summary>
        /// 认证方案：这是一个已知中间件的值，当有多个实例的中间件如果你想限制授权到一个实例时这个选项将会起作用。
        /// </summary>
        public static string AuthenticationScheme => "Authentication";

        public static string ClaimsIssuer => "ghm";
        /// <summary>
        /// 登录路径
        /// </summary>
        public static PathString LoginPath => new PathString("/api/...");
        /// <summary>
        /// 退出路径
        /// </summary>
        public static PathString LogoutPath => new PathString("/logout");

        /// <summary>
        /// //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径
        /// </summary>
        public static PathString AccessDeniedPath => new PathString("/page-not-found");

    }
}
