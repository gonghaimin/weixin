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
    /// 图片消息
    /// </summary>
    public class RequestMessageImage : RequestMessageBase
    {
        public RequestMessageImage(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public string MediaId { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.image;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.PicUrl = element.Element("PicUrl").Value;
                    this.MediaId = element.Element(Common.MediaId).Value;
                }
            }
        }
    }
}
