using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Cookie
{
    public static class CookieAuthRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthService),typeof(CookieAuthenticationService));
        }
        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            builder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = CookieAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }
    }
}
