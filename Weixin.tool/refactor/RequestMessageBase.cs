using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Utility;

namespace Weixin.Tool.refactor
{
    /// <summary>
    /// 微信消息基类
    /// </summary>
    public abstract class RequestMessageBase: MessageBase,IRequestMessageBase
    {
        public abstract RequestMsgType MsgType { get; }
        public long MsgId { get; set; }

    }
}
