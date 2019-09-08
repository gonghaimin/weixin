using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.WxResults
{
    public abstract class BaseJsonResult : IJsonResult
    {

        public virtual string errmsg { get; set; }
        public abstract ReturnCode ReturnCode { get; }
        public virtual int errcode { get; set; }

    }
}
