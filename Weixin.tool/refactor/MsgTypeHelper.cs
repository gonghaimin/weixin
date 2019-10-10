using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    /// <summary>
    /// 消息类型帮助类
    /// </summary>
    public static class MsgTypeHelper
    {
        /// <summary>
        /// 根据xml信息，返回RequestMsgType
        /// </summary>
        /// <returns></returns>
        public static string GetRequestMsgTypeString(XDocument requestMessageDocument)
        {
            if (requestMessageDocument == null || requestMessageDocument.Root == null || requestMessageDocument.Root.Element("MsgType") == null)
            {
                return "Unknow";
            }
            return requestMessageDocument.Root.Element("MsgType").Value;
        }

        /// <summary>
        /// 根据xml信息，返回RequestMsgType
        /// </summary>
        /// <returns></returns>
        public static RequestMsgType GetRequestMsgType(XDocument requestMessageDocument)
        {
            return MsgTypeHelper.GetRequestMsgType(MsgTypeHelper.GetRequestMsgTypeString(requestMessageDocument));
        }

        /// <summary>
        /// 根据xml信息，返回RequestMsgType
        /// </summary>
        /// <returns></returns>
        public static RequestMsgType GetRequestMsgType(string str)
        {
            RequestMsgType result;
            try
            {
                result = (RequestMsgType)Enum.Parse(typeof(RequestMsgType), str, true);
            }
            catch
            {
                result = RequestMsgType.Unknown;
            }
            return result;
        }

        /// <summary>
        /// 根据xml信息，返回ResponseMsgType
        /// </summary>
        /// <returns></returns>
        public static ResponseMsgType GetResponseMsgType(XDocument doc)
        {
            return MsgTypeHelper.GetResponseMsgType(doc.Root.Element("MsgType").Value);
        }

        /// <summary>
        /// 根据xml信息，返回ResponseMsgType
        /// </summary>
        /// <returns></returns>
        public static ResponseMsgType GetResponseMsgType(string str)
        {
            return (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), str, true);
        }
    }
}
