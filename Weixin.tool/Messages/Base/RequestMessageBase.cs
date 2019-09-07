using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 微信消息基类
    /// </summary>
    public abstract class RequestMessageBase: MessageBase
    {
        public RequestMessageBase(string xml)
        {
            this.CreateRequestMessageFromXml(xml);
        }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public abstract RequestMsgType MsgType { get; }


        protected void CreateRequestMessageFromXml(string xml) 
        {
            this.PerfectMessage(xml);
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    this.FromUserName = element.Element(Common.FromUserName).Value;
                    this.ToUserName = element.Element(Common.ToUserName).Value;
                    this.CreateTime =long.Parse( element.Element(Common.CreateTime).Value);
                    this.MsgId = element.Element(Common.MsgId)?.Value;

                }
            }
        }
        /// <summary>
        /// 完善message其他属性
        /// </summary>
        /// <param name="xml"></param>
        protected abstract void PerfectMessage(string xml);
    }
}
