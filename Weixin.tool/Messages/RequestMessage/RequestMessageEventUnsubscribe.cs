using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 取消关注
    /// </summary>
    public class RequestMessageEventUnsubscribe : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {

        /// <summary>
        /// 事件类型，subscribe(订阅)
        /// </summary>
        public override Event Event => Event.unsubscribe;
    }
}
