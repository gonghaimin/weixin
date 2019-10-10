using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{

    /// <summary>
    /// RequestMessage 消息处理方法工厂类
    /// </summary>
    public static class RequestMessageFactory
    {
        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(XDocument doc)
        {
            RequestMessageBase requestMessageBase = null;
            try
            {
                RequestMsgType requestMsgType = MsgTypeHelper.GetRequestMsgType(doc);
                switch (requestMsgType)
                {
                    case RequestMsgType.text:
                        requestMessageBase = new RequestMessageText();
                        break;
                    case RequestMsgType.location:

                        break;
                    case RequestMsgType.image:
                        requestMessageBase = new RequestMessageImage();
                        break;
                    case RequestMsgType.voice:
                        requestMessageBase = new RequestMessageVoice();
                        break;
                    case RequestMsgType.video:
                        requestMessageBase = new RequestMessageVideo();
                        break;
                    case RequestMsgType.link:

                        break;
                    case RequestMsgType.shortvideo:

                        break;
                    case RequestMsgType.@event:
                        Event eventType = EventHelper.GetEventType(doc);
                        switch (eventType)
                        {
                            case Event.LOCATION:
                                break;
                            default:
                                requestMessageBase = new RequestMessageEventBase();
                                break;
                        }
                        break;
                    default:
                        throw new ArgumentException("消息类型不能处理");

                }
                EntityHelper.FillEntityWithXml<RequestMessageBase>(requestMessageBase, doc);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(string.Format("RequestMessage转换出错！可能是MsgType不存在！，XML：{0}，error:{1}", doc.ToString(), ex.ToString()));
            }
            return requestMessageBase;
        }

        /// <summary>
        /// 获取XML转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(string xml)
        {
            return RequestMessageFactory.GetRequestEntity(XDocument.Parse(xml), null);
        }

        /// <summary>
        /// 获取内容为XML的Stream转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <param name="stream">如Request.InputStream</param>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(Stream stream)
        {
            IRequestMessageBase requestEntity;
            using (XmlReader xmlReader = XmlReader.Create(stream))
            {
                requestEntity = RequestMessageFactory.GetRequestEntity(XDocument.Load(xmlReader), null);
            }
            return requestEntity;
        }
    }
}
    