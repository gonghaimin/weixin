using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    public class RequestMessageEventLocation : RequestMessageBase
    {
        public RequestMessageEventLocation(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.location;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.Label = element.Element("Label")?.Value;
                    this.Scale = element.Element("Scale")?.Value;
                    this.Location_Y = element.Element("Location_Y")?.Value;
                    this.Location_X = element.Element("Location_X")?.Value;
                }
            }
        }
    }
}
