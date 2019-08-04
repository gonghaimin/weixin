using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Core.Data;
using Weixin.Core.Domain;
using Weixin.Data;
using Weixin.Services;
using Weixin.WebApi.Policy;

namespace Weixin.WebApi.Extensions
{
    public static class DependencyRegister
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {

            string basePath = System.IO.Directory.GetCurrentDirectory();
            var connections = configuration.GetSection("Connections");
            var connStr = "Data source=" + Path.Combine(basePath, "mydb.db");
            services.AddDbContext<DbContext, WeixinContext>(options => options.UseSqlite(connStr));
            services.AddSingleton(typeof(IHttpContextAccessor));
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));
            //services.AddScoped(typeof(INoteService), typeof(NoteService));
            //services.AddScoped(typeof(ISecurityService), typeof(SecurityService));
            //services.AddScoped(typeof(IAuthenticationService), typeof(CookieAuthenticationService));
            //services.AddScoped(typeof(IWorkContext), typeof(WorkContext));
            //services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));

            //services.ConfigureApplicationCookie(options=>options.)
            services.AddSingleton(typeof(AppsettingsUtility));

            services.AddAuthorization(option =>
            {
                #region 自定义验证策略
                //在需要验证的Controller或者Action中加上[Authorize(Policy = "common")]属性
                option.AddPolicy("common", policy => policy.Requirements.Add(new CommonAuthorize()));
                #endregion
            });
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                if (!string.IsNullOrEmpty(configuration["JwtOption:SecurityKey"]))
                {
                    TokenContext.securityKey = configuration["JwtOption:SecurityKey"];
                }
                //设置需要验证的项目
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "http://localhost:5200",
                    ValidAudience = "api",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenContext.securityKey))//拿到SecurityKey
                                                                                                                 /***********************************TokenValidationParameters的参数默认值***********************************/
                                                                                                                 // ValidateIssuer = true,//是否验证Issuer
                                                                                                                 //ValidateAudience = true,//是否验证Audience
                                                                                                                 //ValidateLifetime = true,//是否验证失效时间
                                                                                                                 //ValidateIssuerSigningKey = true,//是否验证SecurityKey
                                                                                                                 //ValidAudience = "igbom_web",//Audience
                                                                                                                 //ValidIssuer = "igbom_web",//Issuer，这两项和前面签发jwt的设置一致                            
                                                                                                                 // RequireExpirationTime = true,     是否要求Token的Claims中必须包含Expires                                        
                                                                                                                 //   ClockSkew = TimeSpan.FromSeconds(300),       允许的服务器时间偏移量                       
                                                                                                                                                                                                                   
                };
            });

            //自定义策略IOC添加

            services.AddSingleton<IAuthorizationHandler, CommonAuthorizeHandler>();

        }
    }
}
//https://www.cnblogs.com/RainingNight/p/jwtbearer-authentication-in-asp-net-core.html
//https://www.jianshu.com/p/9c3b6ede9ac8
//https://blog.csdn.net/csdn296/article/details/80902580
//https://www.cnblogs.com/RainingNight/p/jwtbearer-authentication-in-asp-net-core.html
