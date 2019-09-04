using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 事件消息
    /// </summary>
    public class EventMessage : BaseMessage
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public EventMessage()
        {
            this.MsgType = MsgTypeEnum.@event.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static EventMessage LoadFromXml(string xml)
        {
            EventMessage em = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    em = new EventMessage();
                    em.FromUserName = element.Element(Common.FromUserName).Value;
                    em.ToUserName = element.Element(Common.ToUserName).Value;
                    em.CreateTime = element.Element(Common.CreateTime).Value;
                    em.Event =element.Element("Event")?.Value;
                    em.EventKey = element.Element("EventKey")?.Value;
                    em.Ticket = element.Element("Ticket")?.Value;
                    em.Latitude = element.Element("Latitude")?.Value;
                    em.Longitude = element.Element("Longitude")?.Value;
                    em.Precision = element.Element("Precision")?.Value; 
                }
            }

            return em;
        }
    }
}
