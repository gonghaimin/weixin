using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 接收到请求的消息基类
    /// </summary>
    public class RequestMessageBase : MessageBase, IRequestMessageBase, IMessageBase
    {
        public long MsgId
        {
            get;
            set;
        }

        public virtual RequestMsgType MsgType => RequestMsgType.text;

        public string Encrypt
        {
            get;
            set;
        }
    }
}
