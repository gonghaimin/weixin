using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Weixin.Core.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;

namespace AuthService.Cookie
{
    public class CookieAuthenticationService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;
        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor, IDistributedCache cache)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._cache = cache;
        }

        public User GetCurrentUser()
        {
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            User user = null;

            var userNameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name && claim.Issuer.Equals(CookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if(userNameClaim!=null)
            {
                user = new User { FullName = userNameClaim.Value };
            }

            return user;
        }

        public void SignIn(User user)
        {
            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(30);

            //将用户信息添加到 Claim 中
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.RoleId.ToString()),
                new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };
            identity.AddClaims(claims);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };

            _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authenticationProperties).Wait();
        }

        public void SignOut()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
        }

    }
}
