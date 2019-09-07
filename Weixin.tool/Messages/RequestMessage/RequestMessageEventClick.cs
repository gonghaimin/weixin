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
    public class RequestMessageEventClick : RequestMessageBase
    {
        public RequestMessageEventClick(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 事件类型，CLICK
        /// </summary>
        public Event Event { get; set; } = Event.CLICK;
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }

        public override RequestMsgType MsgType { get => RequestMsgType.@event;}

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Event =element.Element("Event").Value.StringConvertToEnum<Event>();
                    this.EventKey = element.Element("EventKey").Value;
                }
            }
        }
    }
}
