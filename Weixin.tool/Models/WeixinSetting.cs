using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{
    public class WeixinSetting
    {
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string Oauth_redirect_uri { get; set; }
    }
}
