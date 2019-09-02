using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    public class VoiceMessage: Message
    {
        public string MsgId { get; set; }
        public string Format { get; set; }
        public string MediaId { get; set; }
        public VoiceMessage()
        {
            this.MsgType = MsgTypeEnum.voice.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static VoiceMessage LoadFromXml(string xml)
        {
            VoiceMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new VoiceMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.Format = element.Element(Common.Format).Value;
                    m.MediaId = element.Element(Common.MediaId).Value;
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
                                  <Format><![CDATA[{4}]]></Format>
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
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.Format, this.MediaId, this.MsgId);
        }
    }
}
