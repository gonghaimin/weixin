using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleAuthenticatorWeb.Models
{
    public class LoginUser
    {
        /// <summary>
        /// 账号
        /// </summary>
        [DisplayName("账号")]
        [Required, StringLength(6)]
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        [Required, StringLength(6)]
        public string PassWord { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        [DisplayName("秘钥")]
        public string SecretKey { get; set; }
    }
}
