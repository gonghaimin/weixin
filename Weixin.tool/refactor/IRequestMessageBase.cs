using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public interface IRequestMessageBase:IMessageBase
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        RequestMsgType MsgType { get; }
        /// <summary>
        /// 消息Id
        /// </summary>
        long MsgId { get; set; }
    }
}
