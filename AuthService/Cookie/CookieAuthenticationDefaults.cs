using Microsoft.AspNetCore.Http;


namespace AuthService.Cookie
{
    public class CookieAuthenticationDefaults
    {
        public static string AuthenticationScheme => "Authentication";

        public static string ClaimsIssuer => "ghm";
        /// <summary>
        /// 当cookie过期或无效时，重定向目标
        /// </summary>
        public static PathString LoginPath => new PathString("/api/...");

        public static PathString LogoutPath => new PathString("/logout");

        public static PathString AccessDeniedPath => new PathString("/page-not-found");

    }
}
