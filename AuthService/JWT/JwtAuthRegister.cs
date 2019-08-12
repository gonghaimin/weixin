using AuthService.JWT.Policy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Weixin.Core.Options;

namespace AuthService.JWT
{
    //https://www.cnblogs.com/danvic712/p/10331976.html
    public static class JwtAuthRegister
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var expire = Convert.ToDouble(configuration["JwtOption:Expiration"]);
            var securityKey = configuration["JwtOption:SecurityKey"];
            var issuer = configuration["JwtOption:Issuer"];
            var audience = configuration["JwtOption:Audience"];

            TimeSpan expiration = TimeSpan.FromMinutes(expire);
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            services.AddAuthorization(options =>
            {
                //在需要验证的Controller或者Action中加上[Authorize(Policy = "common")]属性
                options.AddPolicy("common", policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(s =>
            {
                //2、Authentication
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>
            {
                //3、Use Jwt bearer 
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key,
                    ClockSkew = expiration,
                    ValidateLifetime = true
                };
                s.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddSingleton<IJwtAppService, JwtAppService>();
            //DI handler process function
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
        }
    }
}
