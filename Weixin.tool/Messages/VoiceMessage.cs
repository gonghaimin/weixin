using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 语音消息 ，可用于被动回复
    /// </summary>
    public class VoiceMessage: IReplyMessage
    {
        /// <summary>
        /// 语音格式：amr
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音消息媒体id，可以调用获取临时素材接口拉取该媒体
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }
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
                    m.Format = element.Element("Format")?.Value;
                    m.MediaId = element.Element(Common.MediaId)?.Value;
                }
            }

            return m;
        }
        /// <summary>
        /// 模板
        /// </summary>
        public string Template
        {
            get
            {
                return @"<xml>
                                  <ToUserName><![CDATA[{0}]]></ToUserName>
                                  <FromUserName><![CDATA[{1}]]></FromUserName>
                                  <CreateTime>{2}</CreateTime>
                                  <MsgType><![CDATA[{3}]]></MsgType>
                                  <Voice>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                   </Voice>
                            </xml>";
            }
        }
        protected override bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(this.MediaId))
            {
                msg = "MediaId";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 生成回复内容
        /// </summary>
        /// <returns></returns>
        protected override string GenerateContent()
        {
            this.CreateTime = Common.GetNowTime();
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.MediaId);
        }
    }
}
