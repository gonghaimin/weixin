using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
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
        public static IHandler CreateHandler(string requestXml, SignModel model)
        {
            IHandler handler = null;
            if (!string.IsNullOrEmpty(requestXml))
            {
             
                if(model != null)
                {
                    MsgCryptUtility mc = new MsgCryptUtility(Common.Token, Common.encodingAESKey, Common.AppID);
                    string sMsg = "";  //解析之后的明文
                    int ret = 0;
                    ret = mc.DecryptMsg(model.msg_signature, model.timestamp, model.nonce, requestXml,ref sMsg);
                    if(ret==0 && !string.IsNullOrEmpty(sMsg))
                    {
                        requestXml = sMsg;
                    }
                    else
                    {
                        throw new Exception("解密失败");
                    }
                }

                //解析数据
                XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(requestXml);
                XmlNode node = doc.SelectSingleNode("/xml/MsgType");
                if (node != null)
                {
                    XmlCDataSection section = node.FirstChild as XmlCDataSection;
                    if (section != null)
                    {
                        string msgType = section.Value;

                        switch (Enum.Parse(typeof(MsgTypeEnum),msgType))
                        {
                            case MsgTypeEnum.text:
                                handler = new TextHandler(requestXml,model);
                                break;
                            case MsgTypeEnum.@event:
                                handler = new EventHandler(requestXml);
                                break;
                            case MsgTypeEnum.location:
                                handler = new LocationHandler(requestXml);
                                break;
                            default:
                                handler = new TextHandler(requestXml, model);
                                break;
                        }
                    }
                }
            }

            return handler;
        }
    }
}
