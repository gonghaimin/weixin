using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{
    /// <summary>
    /// 客服发送消息
    /// </summary>
   public  class CustomSend
    {
        /// <summary>
        /// 接收者openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 文本消息
        /// </summary>
        public Dictionary<string,string> text { get; set; }
    }
}
