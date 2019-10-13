using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ResponseMessageText : ResponseMessageBase, IResponseMessageBase, IMessageBase
    {
        public override ResponseMsgType MsgType => ResponseMsgType.Text;

        public string Content
        {
            get;
            set;
        }

        public ResponseMessageText()
        {
        }
    }
}
