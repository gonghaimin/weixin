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
    /// 小视频消息
    /// </summary>
    public class RequestMessageShortVideo : RequestMessageBase
    {

        public RequestMessageShortVideo(string xml) : base(xml)
        {
        }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 视频消息媒体id，可以调用获取临时素材接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.shortvideo;

        protected override void PerfectMessage(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.ThumbMediaId = element.Element("ThumbMediaId")?.Value;
                    this.MediaId = element.Element(Common.MediaId)?.Value;
                }
            }
        }
    }
}
