using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public interface IResponseMessageBase:IMessageBase
    {
        ResponseMsgType MsgType { get; }
    }
}
