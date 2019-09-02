using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    public class LocationMessage: Message
    {
        public string MsgId { get; set; }
        public string Label { get; set; }
        public string Scale { get; set; }
        public string Location_Y { get; set; }
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
                    m.Label = element.Element(Common.Format).Value;
                    m.Scale = element.Element(Common.Label).Value;
                    m.Location_Y = element.Element(Common.Location_Y).Value;
                    m.Location_X = element.Element(Common.Location_X).Value;
                }
            }

            return m;
        }
        /// <summary>
        /// 模板
        /// </summary>
        public override string Template
        {
            get
            {
                return @"<xml>
                                  <ToUserName><![CDATA[{0}]]></ToUserName>
                                  <FromUserName><![CDATA[{1}]]></FromUserName>
                                  <CreateTime>{2}</CreateTime>
                                  <MsgType><![CDATA[{3}]]></MsgType>
                                  <Location_X>{4}</Location_X>
                                  <Location_Y>{5}</Location_Y>
                                  <Scale>{6}</Scale>
                                  <Label><![CDATA[{7}]]></Label>
                                  <MsgId>{8}</MsgId>
                            </xml>";
            }
        }
        /// <summary>
        /// 生成内容
        /// </summary>
        /// <returns></returns>
        public override string GenerateContent()
        {
            this.CreateTime = Common.GetNowTime();
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.Location_X, this.Location_Y, this.Scale,this.Label,this.MsgId);
        }
    }
}
