using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 所有Request和Response消息的接口
    /// </summary>
    public interface IMessageBase
    {
        /// <summary>
        /// 接收人（在 Request 中为公众号的微信号，在 Response 中为 OpenId）
        /// </summary>
        string ToUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人（在 Request 中为OpenId，在 Response 中为公众号的微信号）
        /// </summary>
        string FromUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        DateTimeOffset CreateTime
        {
            get;
            set;
        }
    }
}
