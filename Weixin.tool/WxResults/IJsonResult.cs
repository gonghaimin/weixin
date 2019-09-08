using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.WxResults
{
    public interface IJsonResult
    {
        string errmsg
        {
            get;
            set;
        }
        int errcode
        {
            get;
            set;
        }
    }
}
