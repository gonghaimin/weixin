using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Messages.Base
{
    public class RequestMessageEventBase : RequestMessageBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.@event;

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual Event Event => Event.CLICK;

        public virtual string EventKey { get; set; }

        public RequestMessageEventBase()
        {
        }
    }
}
