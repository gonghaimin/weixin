using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Jwt
{
    //https://www.cnblogs.com/danvic712/p/10331976.html
    public static class JwtConfig
    {
        public static void ConfigureServices(this IServiceCollection services, IOptions<JwtOption> jwtOption)
        {
            TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(jwtOption.Value.Expiration));
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Value.IssuerSigningKey));

            services.AddAuthorization(options =>
            {
                //1、Definition authorization policy
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(new PolicyRequirement()));
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
                    ValidIssuer = jwtOption.Value.ValidIssuer,
                    ValidAudience = jwtOption.Value.ValidAudience,
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

            //DI handler process function
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();

          

        }
    }
}
