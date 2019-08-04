using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Weixin.Data;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using Weixin.WebApi.Extensions;
using Weixin.Core.Infranstructure;
using Weixin.Core.Domain;
using Weixin.Core.Data;
using Weixin.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Weixin.WebApi.Options;

namespace Weixin.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info() { Title = "Swagger Test UI", Version = "v1" });
                options.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                options.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "Weixin.WebApi.xml")); // 标注要使用的 XML 文档
            });

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
            services.Configure<JwtOption>(Configuration.GetSection("JwtOption"));
            EngineContext.Create(services.BuildServiceProvider());
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<JwtCustomerAuthorizeMiddleware>(new List<string>() { "/api/values/getjwt", "/" });
            app.UseAuthentication();//启动配置权限管道
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //访问swagger
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MsSystem API V1");
            });
        }
    }
}
