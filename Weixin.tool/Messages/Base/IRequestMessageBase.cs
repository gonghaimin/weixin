using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 请求消息基础接口
    /// </summary>
    public interface IRequestMessageBase : IMessageBase
    {
        long MsgId
        {
            get;
            set;
        }

        RequestMsgType MsgType
        {
            get;
            set;
        }

        string Encrypt
        {
            get;
            set;
        }
    }
}
