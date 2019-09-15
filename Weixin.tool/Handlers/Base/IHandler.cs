using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers.Base
{
    /// <summary>
    /// 处理接口
    /// </summary>
    public abstract class IHandler
    {
        /// <summary>
        /// 请求的XML
        /// </summary>
        public string RequestXml { get; set; }
        /// <summary>
        /// 请求签名实体，用于接受消息解密和回复消息加密
        /// </summary>
        public SignModel SignModel { get; set; }

        /// <summary>
        /// 1、处理请求。
        /// 2、一定要明确：回复的消息类型不一定要与请求的消息类型一样。
        /// 3、但必须是可用于回复的，当前支持的有：文本、图文、音乐等。
        /// </summary>
        /// <returns></returns>
        public abstract string HandleRequest();

        public virtual string HandleRequest(ResponseMessageBase responseMessage)
        {
            if (!string.IsNullOrEmpty(this.RequestXml))
            {
                XElement element = XElement.Parse(this.RequestXml);
                if (element != null)
                {
                    responseMessage.ToUserName = element.Element(Common.FromUserName).Value;
                    responseMessage.FromUserName = element.Element(Common.ToUserName).Value;
                    responseMessage.CreateTime = Common.GetNowTime();
                    return responseMessage.GetResponse(this.SignModel);
                }
            }
            throw new Exception("接收消息有误");
        }
    }
}
