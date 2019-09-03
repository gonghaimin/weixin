using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class LinkMessage : BaseMessage
    {
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        public LinkMessage()
        {
            this.MsgType = MsgTypeEnum.link.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static LinkMessage LoadFromXml(string xml)
        {
            LinkMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new LinkMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.Url = element.Element("Url").Value;
                    m.Title = element.Element("Title").Value;
                    m.Description = element.Element("Description").Value;
                }
            }

            return m;
        }

    }
}
