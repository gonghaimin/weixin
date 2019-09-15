using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers.Base
{
    /// <summary>
    /// 处理器工厂类
    /// </summary>
    public class HandlerFactory
    {
        /// <summary>
        /// 创建处理器
        /// </summary>
        /// <param name="requestXml">请求的xml</param>
        /// <returns>IHandler对象</returns>
        public IHandler CreateHandler(string requestXml, SignModel signModel)
        {
            if (signModel != null && !string.IsNullOrEmpty(signModel.msg_signature))
            {
                MsgCryptUtility mc = new MsgCryptUtility(WeiXinContext.Config.Token, WeiXinContext.Config.EncodingAESKey, WeiXinContext.Config.AppID);
                var ret = mc.DecryptMsg(signModel.msg_signature, signModel.timestamp, signModel.nonce, requestXml, ref requestXml);
                if (ret != 0)
                {
                    throw new Exception("消息解密失败");
                }
            }
            IHandler handler = null;
            if (!string.IsNullOrEmpty(requestXml))
            {
                //解析数据
                XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(requestXml);
                XmlNode node = doc.SelectSingleNode("/xml/MsgType");
                if (node != null)
                {
                    XmlCDataSection section = node.FirstChild as XmlCDataSection;
                    if (section != null)
                    {
                        var msgType = section.Value.StringConvertToEnum<RequestMsgType>();
                        
                        switch (msgType)
                        {
                            case RequestMsgType.text:
                                handler = new TextHandler();
                                break;
                            case RequestMsgType.@event:
                                handler = new EventHandler();
                                break;
                            case RequestMsgType.location:
                                handler = new LocationHandler();
                                break;
                            case RequestMsgType.image:
                                handler = new ImageHandler();
                                break;
                            default:
                                handler = new DefaultHandler();
                                break;
                        }
                    }
                }
            }
            handler.RequestXml = requestXml;
            handler.SignModel = signModel;
            return handler;
        }
    }
}
