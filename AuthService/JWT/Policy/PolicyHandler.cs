﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.JWT.Policy
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// jwt 服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="jwtApp"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IJwtAppService jwtApp)
        {
            Schemes = schemes;
            _jwtApp = jwtApp;
        }

        //授权处理
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            //Todo：获取角色、Url 对应关系
            List<Menu> list = new List<Menu> {
                new Menu
                {
                    Role = Guid.Empty.ToString(),
                    Url = "/api/v1.0/Values"
                }
            };

            var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;

            //获取授权方式
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                //验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
                    //判断是否为已停用的 Token
                    if (!await _jwtApp.IsCurrentActiveTokenAsync())
                    {
                        context.Fail();
                        return;
                    }

                    httpContext.User = result.Principal;

                    //判断角色与 Url 是否对应
                    //
                    var url = httpContext.Request.Path.Value.ToLower();
                    var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
                    var menu = list.Where(x => x.Role.Equals(role) && x.Url.ToLower().Equals(url)).FirstOrDefault();

                    if (menu == null)
                    {
                        context.Fail();
                        return;
                    }

                    //判断是否过期
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.UtcNow)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    return;
                }
            }
            context.Fail();
        }

        /// <summary>
        /// 测试菜单类
        /// </summary>
        public class Menu
        {
            public string Role { get; set; }

            public string Url { get; set; }
        }
    }

    //这类主要是承载一些初始化值，然后传递到Handler中去，给验证做逻辑运算提供一些可靠的信息；自己根据自身情况自己定义适当的属性作为初始数据的承载容器；
    public class PolicyRequirement : IAuthorizationRequirement
    {
    }
}
