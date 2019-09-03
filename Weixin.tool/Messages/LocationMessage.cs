using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    public class LocationMessage: Message
    {
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
        public LocationMessage()
        {
            this.MsgType = MsgTypeEnum.location.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static LocationMessage LoadFromXml(string xml)
        {
            LocationMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new LocationMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.Label = element.Element("Label").Value;
                    m.Scale = element.Element("Scale").Value;
                    m.Location_Y = element.Element("Location_Y").Value;
                    m.Location_X = element.Element("Location_X").Value;
                }
            }

            return m;
        }

    }
}
