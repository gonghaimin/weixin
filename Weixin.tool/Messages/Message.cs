﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 微信消息基类
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 发送方帐号，发送方微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 接收方账号，接收方微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; protected set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

    }
}
