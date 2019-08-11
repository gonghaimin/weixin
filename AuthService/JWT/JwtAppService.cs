using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Weixin.Core.Domain;
using Weixin.Core.Options;

namespace AuthService.JWT
{
    public class JwtAppService : IJwtAppService
    {
        /// <summary>
        /// 分布式缓存
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 获取 HTTP 请求上下文
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// jwt json配这种
        /// </summary>
        private readonly IOptions<JwtOption> _jwtOption;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public JwtAppService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor,IOptions<JwtOption> jwtOption)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOption = jwtOption;
        }


        /// <summary>
        /// 新增 Token
        /// </summary>
        /// <param name="user">用户信息数据传输对象</param>
        /// <returns></returns>
        public JwtAuthorizationDto Create(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecurityKey));

            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(_jwtOption.Value.Expiration);

            //将用户信息添加到 Claim 中
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };
            identity.AddClaims(claims);

            //签发一个加密后的用户信息凭证，用来标识用户的身份
            _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//创建声明信息
                Issuer = _jwtOption.Value.Issuer,//Jwt token 的签发者
                Audience = _jwtOption.Value.Audience,//Jwt token 的接收者
                Expires = expiresAt,//过期时间
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)//创建 token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //存储 Token 信息
            var jwt = new JwtAuthorizationDto
            {
                UserId = user.Id,
                Token = tokenHandler.WriteToken(token),
                Auths = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                Expires = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                Success = true
            };
            return jwt;
        }

        /// <summary>
        /// 停用 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task DeactivateAsync(string token)
        {
              await _cache.SetStringAsync(GetKey(token)," ", new DistributedCacheEntryOptions{AbsoluteExpirationRelativeToNow =TimeSpan.FromMinutes(_jwtOption.Value.Expiration) });
        }

        /// <summary>
        /// 停用当前 Token
        /// </summary>
        /// <returns></returns>
        public async Task DeactivateCurrentAsync()=> await DeactivateAsync(GetCurrentAsync());

        /// <summary>
        /// 判断 Token 是否有效
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task<bool> IsActiveAsync(string token)=> await _cache.GetStringAsync(GetKey(token)) == null;

        /// <summary>
        /// 判断当前 Token 是否有效
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsCurrentActiveTokenAsync() => await IsActiveAsync(GetCurrentAsync());

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public async Task<JwtAuthorizationDto> RefreshAsync(User user)
        {
            var jwt = Create(user);
            //停用修改前的 Token 信息
            await DeactivateCurrentAsync();
            return jwt;
        }

        /// <summary>
        /// 设置缓存中过期 Token 值的 key
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private static string GetKey(string token) => $"deactivated token:{token}";

        /// <summary>
        /// 获取 HTTP 请求的 Token 值
        /// </summary>
        /// <returns></returns>
        private string GetCurrentAsync()
        { 
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty? string.Empty: authorizationHeader.Single().Split(" ").Last();
        }

    }
}
