using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Utility
{
    public class CommonApi
    {
        public static GetMenuResult GetMenu()
        {
            return ApiHandlerWapper.TryCommonApi(delegate (string accessToken)
            {
                var result=ApiHandlerWapper.GetJson<GetMenuResult>("https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accessToken);
                return result;
            });
        }
    }
}
