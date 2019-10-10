using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public class RequestMessageEventBase : RequestMessageBase, IRequestMessageEventBase
    {
        public override RequestMsgType MsgType => RequestMsgType.@event;
        public virtual Event Event => Event.CLICK;

        public string EventKey { get; set; }
    }
}
