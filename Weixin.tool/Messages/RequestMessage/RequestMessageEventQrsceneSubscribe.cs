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
    /// 扫描带参数二维码事件, 用户未关注时，进行关注后的事件推送
    /// </summary>
    public class RequestMessageEventQrsceneSubscribe : RequestMessageBase
    {
        public RequestMessageEventQrsceneSubscribe(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 事件类型，subscribe(订阅)
        /// </summary>
        public Event Event { get; set; } = Event.subscribe;
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.@event;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Event = element.Element("Event").Value.StringConvertToEnum<Event>();
                    this.EventKey = element.Element("EventKey").Value;
                    this.Ticket = element.Element("Ticket").Value;
                }
            }
        }
    }
}
