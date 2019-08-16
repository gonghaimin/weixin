using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Cookie
{
    /// <summary>
    /// 在控制器或action标记[Authorize]
    /// </summary>
    public static class CookieAuthRegister
    {
        public static void Register(this IServiceCollection services)
        {
            
        }
        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            //AddCookie 用来注册 CookieAuthenticationHandler，由它来完成身份认证的主要逻辑。
            builder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.Cookie.Name = "Authentication";
                options.Cookie.HttpOnly = true;
                options.LoginPath = CookieAuthenticationDefaults.LoginPath;//登录路径
                options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;//禁止访问路径
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.SlidingExpiration = true;
                //Cookie可以分为永久性的和临时性的。 临时性的是指只在当前浏览器进程里有效，浏览器一旦关闭就失效（被浏览器删除）。 永久性的是指Cookie指定了一个过期时间，在这个时间到达之前，此cookie一直有效（浏览器一直记录着此cookie的存在）。 slidingExpriation的作用是，指示浏览器把cookie作为永久性cookie存储，但是会自动更改过期时间，自动续期当用户一直在活跃中。
            });
        }
    }
}
