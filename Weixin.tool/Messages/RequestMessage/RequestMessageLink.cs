using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class RequestMessageLink : RequestMessageBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.link;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        public RequestMessageLink()
        {
        }
    }
}
