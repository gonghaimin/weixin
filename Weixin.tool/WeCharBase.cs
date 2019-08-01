using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Weixin.Tool
{
    public class WeCharBase
    {
        private static readonly string appId;
        private static readonly string appSecret;
        private static readonly IHttpClientFactory _clientFactory;
        private static readonly HttpClient _client = new HttpClient();
        static WeCharBase()
        {
            appId = "wxc69b92dd79536d58";
            appSecret = "b4ffc8669785efff1fe00275eea1bee5";
        }

        public static string AccessToken
        {
            get { return GetToken(); }
        }


        /// <summary>获取ccess_token</summary>
        /// <retuWeixin></retuWeixin>
        private static string GetToken()
        {
            try
            {
                //var client = _clientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get,
          string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));
                var response = _client.SendAsync(request).Result;
                var str = response.Content.ReadAsStringAsync().Result;
                var dict=JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
                var accessToken = dict["access_token"];
                if (accessToken == null)
                {
                    return string.Empty;
                }
                return accessToken.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
