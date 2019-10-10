using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public interface IRequestMessageEventBase:IRequestMessageBase
    {
        Event Event { get;  }
        string EventKey { get; set; }
    }
}
