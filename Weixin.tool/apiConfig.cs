using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool
{
    public class apiConfig
    {
        public static string tokenApi = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        public static string menuApi = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        
    }
}
