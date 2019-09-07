using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class RequestMessageLink : RequestMessageBase
    {
        public RequestMessageLink(string xml) : base(xml)
        {
        }

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
        public override RequestMsgType MsgType => RequestMsgType.link;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Url = element.Element("Url").Value;
                    this.Title = element.Element("Title").Value;
                    this.Description = element.Element("Description").Value;
                }
            }
        }
    }
}
