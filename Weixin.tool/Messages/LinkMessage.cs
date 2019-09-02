using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    public class LinkMessage : Message
    {
        public string Url { get; set; }
        public string Title { get; set; }
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
                    m.Url = element.Element(Common.Format).Value;
                    m.Title = element.Element(Common.MediaId).Value;
                    m.Description = element.Element(Common.MediaId).Value;
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
                                  <ThumbMediaId><![CDATA[{4}]]></ThumbMediaId>
                                  <MediaId><![CDATA[{5}]]></MediaId>
                                  <MsgId>{6}</MsgId>
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
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.ThumbMediaId, this.MediaId, this.MsgId);
        }
    }
}
