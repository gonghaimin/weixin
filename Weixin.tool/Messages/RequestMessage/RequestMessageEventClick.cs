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
    ///自定义菜单， 点击菜单拉取消息时的事件推送
    /// </summary>
    public class RequestMessageEventClick : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event => Event.CLICK;

    }
}
