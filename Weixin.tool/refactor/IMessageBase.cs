using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.refactor
{
    public interface IMessageBase
    {
        string ToUserName { get; set; }
        string FromUserName { get; set; }
        long CreateTime { get; set; }
    }
}
