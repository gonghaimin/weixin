using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weixin.Core.Options;
using Weixin.Services;


namespace Weixin.WebApi.Policy
{
    public class CommonAuthorizeHandler : AuthorizationHandler<CommonAuthorize>
    {
        private IHttpContextAccessor accssor;
        private UserContext userContext;
        private IOptions<JwtOption> jwtOption;
        public CommonAuthorizeHandler(IHttpContextAccessor accssor, UserContext context, IOptions<JwtOption> jwtOption)
        {
            this.accssor = accssor;
            this.userContext = context;
            this.jwtOption = jwtOption;
        }
        /// <summary>
        /// 常用自定义验证策略，模仿自定义中间件JwtCustomerauthorizeMiddleware的验证范围
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CommonAuthorize requirement)
        {
            var httpContext = accssor.HttpContext;
            //var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;

            #region 身份验证，并设置用户Ruser值

            var result = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!result || string.IsNullOrEmpty(authStr.ToString()))
            {
                return Task.CompletedTask;
            }
            result = TokenContext.Validate(authStr.ToString().Substring("Bearer ".Length).Trim(), payLoad =>
            {
                var success = true;
                //可以添加一些自定义验证，用法参照测试用例
                //验证是否包含aud 并等于 roberAudience
                success = success && payLoad["aud"]?.ToString() == jwtOption.Value.Audience;
                if (success)
                {
                    //设置Ruse值,把user信息放在payLoad中，（在获取jwt的时候把当前用户存放在payLoad的ruser键中）
                    //如果用户信息比较多，建议放在缓存中，payLoad中存放缓存的Key值
                    userContext.TryInit(payLoad["ruser"]?.ToString());
                }
                return success;
            });
            if (!result)
            {
                return Task.CompletedTask;
            }

            #endregion
            #region 权限验证
            if (!userContext.Authorize(httpContext.Request.Path))
            {
                return Task.CompletedTask;
            }
            #endregion

            //context.Succeed(requirement);是验证成功，如果没有这个，就默认验证失败
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
//https://www.cnblogs.com/jackyfei/p/9961099.html