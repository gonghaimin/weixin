using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Weixin.WebApi.Extensions;
using Weixin.Core.Infranstructure;
using Weixin.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Weixin.Core.Options;
using AuthService.JWT;
using AuthService;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool;
using Weixin.Tool.Utility;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;

namespace Weixin.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info() { Title = "Swagger Test UI", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                options.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "Weixin.WebApi.xml")); // 标注要使用的 XML 文档
                                                                                                                             //启用auth支持
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

            });
            services.AddHttpClient();
            services.Configure<WeixinSetting>(Configuration.GetSection("WeiXin"));
            services.AddScoped<HandlerFactory>();
            services.AddScoped(typeof(IAuthService), typeof(CookieAuthenticationService));
            services.Register(Configuration);
            services.AddCors();

            //初始化MyOwnModel实例并且映射appSettings里的配置
            services.AddOptions();
            services.Configure<MyOwnModel>(Configuration.GetSection("MyOwn"));
            //通过name注入不同options服务
            services.Configure<MyOwnModel>("自定义配置", model => {
                model.Age = 1;
                model.Name = "dsdf";
            });
            services.AddDistributedRedisCache(r =>
            {
                r.Configuration = Configuration["Redis:ConnectionString"];
            });
            services.AddHttpClient();
            services.AddSession();
            //Configuration.GetSection("").
            services.Configure<JwtOption>(Configuration.GetSection("JwtOption"));

            EngineContext.Create(services.BuildServiceProvider());
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<WeixinSetting> settings, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    services.GetRequiredService<AppsettingsUtility>();//生成全局单利

                    var context = services.GetRequiredService<DbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //Microsoft.AspNetCore.Authentication.AuthenticationMiddleware
            //启动配置权限管道
            //UseAuthentication方法注册了AuthenticationMiddleware中间件
            app.UseAuthentication();

            WeiXinContext.RegisterWX(settings, serviceProvider);

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //访问swagger
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MsSystem API V1");
            });
            app.UseSession();
            app.UseMvc();
        }
    }
}
