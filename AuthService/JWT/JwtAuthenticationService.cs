using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Weixin.Core.Domain;
using Weixin.Core.Options;

namespace AuthService.JWT
{
    public class JwtAuthenticationService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOption> _jwtOption;
        private readonly IDistributedCache _cache;
        public JwtAuthenticationService(IHttpContextAccessor httpContextAccessor, IOptions<JwtOption> jwtOption, IDistributedCache cache)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._cache = cache;
        }
        public User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public void SignIn(User user)
        {
           
            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(_jwtOption.Value.Expiration);

            //将用户信息添加到 Claim 中
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),
                new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };
            identity.AddClaims(claims);
            
            //签发一个加密后的用户信息凭证，用来标识用户的身份
            _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)).Wait();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecurityKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//创建声明信息
                Issuer = _jwtOption.Value.Issuer,//Jwt token 的签发者
                Audience = _jwtOption.Value.Audience,//Jwt token 的接收者
                Expires = expiresAt,//过期时间
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)//创建 token
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", token);

            var str = JsonConvert.SerializeObject(user);
            var key = "user."+user.Id;
            this._cache.SetString(key, str);
        }

        public void SignOut()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme).Wait();
        }
    }
}
