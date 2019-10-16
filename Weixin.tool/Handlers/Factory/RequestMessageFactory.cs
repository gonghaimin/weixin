using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.MsgHelpers;
using Weixin.Tool.Messages.RequestMessage;

namespace Weixin.Tool.Handlers.Factory
{
    /// <summary>
    /// RequestMessage 消息处理方法工厂类
    /// </summary>
    public static class RequestMessageFactory
    {
        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(XDocument doc)
        {
            RequestMessageBase val = null;
            try
            {
                RequestMsgType requestMsgType = MsgTypeHelper.GetRequestMsgType(doc);
                switch (requestMsgType)
                {
                    case RequestMsgType.text:
                        val = new RequestMessageText();
                        break;
                    case RequestMsgType.location:
                        val = new RequestMessageLocation();
                        break;
                    case RequestMsgType.image:
                        val = new RequestMessageImage();
                        break;
                    case RequestMsgType.voice:
                        val = new RequestMessageVoice();
                        break;
                    case RequestMsgType.video:
                        val = new RequestMessageVideo();
                        break;
                    case RequestMsgType.link:
                        val = new RequestMessageLink();
                        break;
                    case RequestMsgType.shortvideo:
                        val = new RequestMessageShortVideo();
                        break;
                    case RequestMsgType.@event:
                        var eventType = EventHelper.GetEventType(doc.Root.Element("Event").Value);

                        switch (eventType)
                        {
                            case Event.CLICK:
                                val = new RequestMessageEventClick();
                                break;
                            case Event.LOCATION:
                                val = new RequestMessageEventLocation();
                                break;
                            case Event.subscribe:
                                val = new RequestMessageEventSubscribe();
                                break;
                            case Event.scan:
                                val = new RequestMessageEventScan();
                                break;
                            case Event.unsubscribe:
                                val = new RequestMessageEventUnsubscribe();
                                break;
                            case Event.VIEW:
                                val = new RequestMessageEventView();
                                break;
                            default:
                                val = new RequestMessageEventBase();
                                break;
                        }
                        break;
                }
                val.FillEntityWithXml<RequestMessageBase>(doc);
                return val;
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"RequestMessage转换出错！可能是MsgType不存在！，XML：{doc.ToString()}",ex);
            }
        }

        /// <summary>
        /// 获取XML转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(string xml)
        {
            return GetRequestEntity(XDocument.Parse(xml));
        }

        /// <summary>
        /// 获取内容为XML的Stream转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <param name="stream">如Request.InputStream</param>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return GetRequestEntity(XDocument.Load(reader));
            }
        }
    }
}
