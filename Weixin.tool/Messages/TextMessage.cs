using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 文本消息，可用于被动回复用户消息
    /// </summary>
    public class TextMessage: IReplyMessage
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TextMessage()
        {
            this.MsgType = MsgTypeEnum.text.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static TextMessage LoadFromXml(string xml)
        {
            TextMessage tm = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    tm = new TextMessage();
                    tm.FromUserName = element.Element(Common.FromUserName).Value;
                    tm.ToUserName = element.Element(Common.ToUserName).Value;
                    tm.CreateTime = element.Element(Common.CreateTime).Value;
                    tm.Content = element.Element(Common.Content).Value;
                    tm.MsgId = element.Element(Common.MsgId).Value;
                }
            }

            return tm;
        }

        public string Template
        {
            get
            {
                return @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[{3}]]></MsgType>
                                <Content><![CDATA[{4}]]></Content>
                            </xml>";
            }
        }
        protected override bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(this.Content))
            {
                msg = "Content";
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
            return string.Format(this.Template,this.ToUserName,this.FromUserName,this.CreateTime,this.MsgType,this.Content);
        }
    }
}
