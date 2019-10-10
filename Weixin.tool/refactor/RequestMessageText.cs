using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.text;
        public string Content { get; set; }
    }
}
