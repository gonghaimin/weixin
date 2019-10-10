using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public class RequestMessageEventSubscribe:RequestMessageEventBase,IRequestMessageEventBase
    {
        public override Event Event => Event.subscribe;
    }
}
