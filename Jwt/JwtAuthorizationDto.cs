using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt
{
    public class JwtAuthorizationDto
    {
        /// <summary>
        /// 授权时间
        /// </summary>
        public long Auths { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// 是否授权成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户主键
        /// </summary>
        public Guid UserId { get; set; }
    }
    public class UserDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Guid Role { get; set; }
    }
}
