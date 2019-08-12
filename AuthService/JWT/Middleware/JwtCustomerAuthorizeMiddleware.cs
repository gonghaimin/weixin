using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weixin.Core.Options;



namespace AuthService.JWT.Middleware
{
    /// <summary>
    /// 自定义授权中间件
    /// </summary>
    public class JwtCustomerAuthorizeMiddleware
    {
        private readonly RequestDelegate next;

        public JwtCustomerAuthorizeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IAuthContext userContext, IOptions<JwtOption> optionContainer)
        {
            //检查当前url是否可以匿名访问，如果可以就直接通过，不做验证了；如果不是可以匿名访问的路径，那就继续
            if (userContext.IsAllowAnonymous(context.Request.Path))
            {
                await next(context);
                return;
            }

            var option = optionContainer.Value;

            #region   设置自定义jwt 的秘钥
            if (!string.IsNullOrEmpty(option.SecurityKey))
            {
                JwtHandler.securityKey = option.SecurityKey;
            }
            #endregion

            #region 身份验证，并设置用户Ruser值

            //获取当前http头部携带的jwt（存放在头部的 Authorization中）
            var result = context.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!result || string.IsNullOrEmpty(authStr.ToString()))
            {
                throw new UnauthorizedAccessException("未授权");
            }
            result = JwtHandler.Validate(authStr.ToString().Substring("Bearer ".Length).Trim(), payLoad =>
            {
                var success = true;
                //可以添加一些自定义验证，用法参照测试用例
                //验证是否包含aud 并等于 roberAudience
                success = success && payLoad["aud"]?.ToString() == option.Audience;
                if (success)
                {
                    //在获取jwt的时候把当前用户存放在payLoad的user键中
                    //如果用户信息比较多，建议放在缓存中，payLoad中存放缓存的Key值
                    //获取当前访问用户信息，把用户的基本信息放在payLoad["user"]中
                    userContext.TryInit(payLoad["user"]?.ToString());
                }
                return success;
            });
            if (!result)
            {
                throw new UnauthorizedAccessException("未授权");
            }

            #endregion
            #region 权限验证
            if (!userContext.Authorize(context.Request.Path))
            {
                throw new UnauthorizedAccessException("未授权");
            }
            #endregion

            await next(context);
        }

     
    }
}
