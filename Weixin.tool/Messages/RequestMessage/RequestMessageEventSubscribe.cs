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
    /// 关注事件
    /// </summary>
    public class RequestMessageEventSubscribe : RequestMessageBase
    {
        public RequestMessageEventSubscribe(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 事件类型，subscribe(订阅)
        /// </summary>
        public Event Event { get; set; } = Event.scan;
        public override RequestMsgType MsgType => RequestMsgType.@event;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Event = element.Element("Event").Value.StringConvertToEnum<Event>();
                }
            }
        }
    }
}
