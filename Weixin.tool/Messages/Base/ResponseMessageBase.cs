using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.MsgHelpers;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 响应回复消息基类
    /// </summary>
    public class ResponseMessageBase : MessageBase, IResponseMessageBase, IMessageBase
    {
        public virtual ResponseMsgType MsgType
        {
            get;
            set;
        }
    }
}