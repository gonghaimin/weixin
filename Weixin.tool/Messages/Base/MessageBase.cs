﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 所有Request和Response消息的基类
    /// </summary>
    public abstract class MessageBase : IMessageBase
    {
        /// <summary>
        /// 接收人（在 Request 中为公众号的微信号，在 Response 中为 OpenId）
        /// </summary>
        public string ToUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人（在 Request 中为OpenId，在 Response 中为公众号的微信号）
        /// </summary>
        public string FromUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTimeOffset CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// ToString() 方法重写
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
