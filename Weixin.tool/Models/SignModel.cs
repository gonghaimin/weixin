using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{
    public class SignModel
    {
        /// <summary>
        /// 签名串
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 时间戳，对应URL参数的timestamp
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 随机串，对应URL参数的nonce
        /// </summary>
        public string nonce { get; set; }
        public string echostr { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string encrypt_type { get; set; }
        /// <summary>
        /// 签名串，对应URL参数的msg_signature
        /// </summary>
        public string msg_signature { get; set; }
    }
}
