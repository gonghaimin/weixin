using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.WxResults
{
    //
    // 摘要:
    //     /// 公众号 JSON 返回结果（用于菜单接口等）
    public class WxJsonResult : BaseJsonResult
    {
        public override int errcode { get; set; } = 0;


        public override ReturnCode ReturnCode
        {
            get
            {
                ReturnCode code=ReturnCode.未知错误;
                Enum.TryParse<ReturnCode>(errcode.ToString(), out code);
                return code;
            }  
        }

        public override string ToString()
        {
            return $"WxJsonResult：{{errcode:'{(int)errcode}',errcode_name:'{ReturnCode.ToString()}',errmsg:'{errmsg}'}}";
        }
    }
}
