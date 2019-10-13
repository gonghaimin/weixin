using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 文本类型消息
    /// </summary>
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.text;

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 点击的菜单ID
        /// <para>收到XML推送之后，开发者可以根据提取出来的bizmsgmenuid和Content识别出微信用户点击的是哪个菜单。</para>
        /// </summary>
        public string bizmsgmenuid
        {
            get;
            set;
        }

        public RequestMessageText()
        {
        }
    }
}