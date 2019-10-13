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
    /// 扫描带参数二维码事件,用户已关注时的事件推送
    /// </summary>
    public class RequestMessageEventScan : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {
        /// <summary>
        /// 事件类型，subscribe(订阅)
        /// </summary>
        public override Event Event=>Event.scan;

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

    }
}
