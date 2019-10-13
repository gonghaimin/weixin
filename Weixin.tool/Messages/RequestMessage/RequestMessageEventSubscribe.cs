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
    /// 关注事件推送
    /// </summary>
    public class RequestMessageEventSubscribe : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {
        /// <summary>
        /// 事件类型，subscribe(订阅)
        /// </summary>
        public override Event Event  =>Event.subscribe;

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

    }
}
