using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.MsgHelpers;
using Weixin.Tool.Messages.ResponseMessage;

namespace Weixin.Tool.Handlers.Factory
{
    /// <summary>
    /// ResponseMessage 消息处理方法工厂类
    /// </summary>
    public static class ResponseMessageFactory
    {
        /// <summary>
        /// 从返回结果XML转换成IResponseMessageBase实体类（通常在反向读取日志的时候用到）。
        /// </summary>
        /// <returns></returns>
        public static IResponseMessageBase GetResponseEntity(XDocument doc)
        {
            ResponseMessageBase val = null;
            try
            {
                ResponseMsgType responseMsgType = MsgTypeHelper.GetResponseMsgType(doc);
                switch (responseMsgType)
                {
                    case ResponseMsgType.Text:
                        val = new ResponseMessageText();
                        break;
                    case ResponseMsgType.Image:
                        val = new ResponseMessageImage();
                        break;
                    case ResponseMsgType.Voice:
                        val = new ResponseMessageVoice();
                        break;
                    case ResponseMsgType.Video:
                        val = new ResponseMessageVideo();
                        break;
                    case ResponseMsgType.Music:
                        val = new ResponseMessageMusic();
                        break;
                    case ResponseMsgType.News:
                        val = new ResponseMessageNews();
                        break;

                }
                val.FillEntityWithXml<ResponseMessageBase>(doc);
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception($"ResponseMessage转换出错！可能是MsgType不存在！，XML：{doc.ToString()}", ex);
            }
        }

        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// </summary>
        /// <returns></returns>
        public static IResponseMessageBase GetResponseEntity(string xml)
        {
            return GetResponseEntity(XDocument.Parse(xml));
        }

        /// <summary>
        /// 将ResponseMessage实体转为XML
        /// </summary>
        /// <param name="entity">ResponseMessage实体</param>
        /// <returns></returns>
        public static XDocument ConvertEntityToXml(IResponseMessageBase entity)
        {
            return EntityHelper.ConvertEntityToXml<IResponseMessageBase>(entity);
        }
        public static string ConvertEntityToXmlStr(IResponseMessageBase entity)
        {
            return EntityHelper.ConvertEntityToXml<IResponseMessageBase>(entity).ToString();
        }
        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <param name="msgType">响应类型</param>
        /// <returns></returns>
        private static IResponseMessageBase CreateFromRequestMessage(ResponseMsgType msgType)
        {
            IResponseMessageBase result = null;
            try
            {
                switch (msgType)
                {
                    case ResponseMsgType.Text:
                        result = new ResponseMessageText();
                        break;
                    case ResponseMsgType.News:
                        result = new ResponseMessageNews();
                        break;
                    case ResponseMsgType.Music:
                        result = new ResponseMessageMusic();
                        break;
                    case ResponseMsgType.Image:
                        result = new ResponseMessageImage();
                        break;
                    case ResponseMsgType.Voice:
                        result = new ResponseMessageVoice();
                        break;
                    case ResponseMsgType.Video:
                        result = new ResponseMessageVideo();
                        break;
                    default:
                        if (msgType != ResponseMsgType.NoResponse)
                        {
                            throw new Exception(string.Format("ResponseMsgType没有为 {0} 提供对应处理程序。", msgType), new ArgumentOutOfRangeException());
                        }
                        result = new ResponseMessageBase();
                        break;
                }
            }
            catch (Exception inner)
            {
                throw new Exception("ResponseMessageFactory.CreateFromRequestMessage过程发生异常", inner);
            }
            return result;
        }

        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <param name="requestMessage">请求数据</param>
        /// <returns></returns>
        public static T CreateFromRequestMessage<T>(IRequestMessageBase requestMessage) where T : IResponseMessageBase
        {
            T result;
            try
            {
                T t = default(T);
                Type typeFromHandle = typeof(T);
                if (typeFromHandle.IsInterface)
                {
                    string value = typeFromHandle.Name.Replace("IResponseMessage", "").Replace("ResponseMessage", "");
                    ResponseMsgType msgType = (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), value);
                    t = (T)CreateFromRequestMessage(msgType);
                }
                else
                {
                    t = (T)((object)Activator.CreateInstance(typeFromHandle));
                }
                if (t != null)
                {
                    t.ToUserName = requestMessage.FromUserName;
                    t.FromUserName = requestMessage.ToUserName;
                    t.CreateTime = DateTime.Now;
                }
                result = t;
            }
            catch (Exception ex)
            {
                throw new Exception("ResponseMessageFactory.CreateFromRequestMessage<T>过程发生异常！", ex);
            }
            return result;
        }
    }
}
