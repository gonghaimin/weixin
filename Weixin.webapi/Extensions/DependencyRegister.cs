using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weixin.Core.Data;
using Weixin.Core.Domain;
using Weixin.Data;

namespace Weixin.WebApi.Extensions
{
    public static class DependencyRegister
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            var connections = configuration.GetSection("Connections");
            //connections["weixin"]
            var conn =System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "mydb.db");
            services.AddDbContext<DbContext,WeixinContext>(options => options.UseSqlite(conn));
            //services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddSingleton(typeof(User));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            //services.AddScoped(typeof(INoteService), typeof(NoteService));
            //services.AddScoped(typeof(ISecurityService), typeof(SecurityService));
            //services.AddScoped(typeof(IAuthenticationService), typeof(CookieAuthenticationService));
            //services.AddScoped(typeof(IWorkContext), typeof(WorkContext));
            //services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));

            //services.ConfigureApplicationCookie(options=>options.)
        }
    }
}
