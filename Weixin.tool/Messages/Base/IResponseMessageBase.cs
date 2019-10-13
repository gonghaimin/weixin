using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 响应回复消息基类接口
    /// </summary>
    public interface IResponseMessageBase : IMessageBase
    {
        ResponseMsgType MsgType
        {
            get;
            set;
        }
    }
}
