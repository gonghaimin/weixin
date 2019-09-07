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
    /// 文本消息
    /// </summary>
    public class RequestMessageText : RequestMessageBase
    {
        public RequestMessageText(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.text;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Content = element.Element(Common.Content)?.Value;
                }
            }
            
        }
    }
}
