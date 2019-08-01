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

            services.Register(Configuration);

            services.AddCors();

            EngineContext.Create(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            app.UseMvc();

        }
    }
}
