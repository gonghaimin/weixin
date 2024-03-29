﻿using System.Threading.Tasks;
using Weixin.Core.Domain;

namespace AuthService.JWT
{
    public interface IJwtAppService
    {
        /// <summary>
        /// 新增 Jwt token
        /// </summary>
        /// <param name="user">用户信息数据传输对象</param>
        /// <returns></returns>
        string Create(User user);

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="user">用户信息数据传输对象</param>
        /// <returns></returns>
        Task<string> RefreshAsync(User user);

        /// <summary>
        /// 判断当前 Token 是否有效
        /// </summary>
        /// <returns></returns>
        Task<bool> IsCurrentActiveTokenAsync();

        /// <summary>
        /// 停用当前 Token
        /// </summary>
        /// <returns></returns>
        Task DeactivateCurrentAsync();

        /// <summary>
        /// 判断 Token 是否有效
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        Task<bool> IsActiveAsync(string token);

        /// <summary>
        /// 停用 Token
        /// </summary>
        /// <returns></returns>
        Task DeactivateAsync(string token);

    }
}
