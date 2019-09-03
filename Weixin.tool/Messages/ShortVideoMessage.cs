using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 小视频消息
    /// </summary>
    public class ShortVideoMessage: BaseMessage
    {
        public ShortVideoMessage()
        {
            this.MsgType = MsgTypeEnum.shortvideo.ToString();
        }
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 视频消息媒体id，可以调用获取临时素材接口拉取数据
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static VideoMessage LoadFromXml(string xml)
        {
            VideoMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new VideoMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.ThumbMediaId = element.Element("ThumbMediaId").Value;
                    m.MediaId = element.Element(Common.MediaId).Value;
                }
            }

            return m;
        }
    }
}
