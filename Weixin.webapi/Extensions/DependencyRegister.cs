using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Weixin.Data;


namespace Weixin.WebApi.Extensions
{
    public static class DependencyRegister
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {

            string basePath = System.IO.Directory.GetCurrentDirectory();
            var connections = configuration.GetSection("Connections");
            var connStr = "Data source=" + Path.Combine(basePath, "mydb.db");
            services.AddDbContext<DbContext, WeixinDbContext>(options => options.UseSqlite(connStr));
            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(WorkContext));
            //services.AddScoped(typeof(INoteService), typeof(NoteService));
            //services.AddScoped(typeof(ISecurityService), typeof(SecurityService));
            //services.AddScoped(typeof(IAuthenticationService), typeof(CookieAuthenticationService));
            //services.AddScoped(typeof(IWorkContext), typeof(WorkContext));
            //services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));

            //services.ConfigureApplicationCookie(options=>options.)
            services.AddSingleton(typeof(AppsettingsUtility));
        }
    }
}
//https://www.cnblogs.com/RainingNight/p/jwtbearer-authentication-in-asp-net-core.html
//https://www.jianshu.com/p/9c3b6ede9ac8
//https://blog.csdn.net/csdn296/article/details/80902580
//https://www.cnblogs.com/RainingNight/p/jwtbearer-authentication-in-asp-net-core.html
