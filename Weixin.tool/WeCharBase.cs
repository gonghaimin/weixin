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

        private static string token = "";
        static WeCharBase()
        {
            appId = "wxc69b92dd79536d58";
            appSecret = "b4ffc8669785efff1fe00275eea1bee5";
        }

  
        public static string createMenu()
        {
            try
            {
                var str = "{\"button\":[{\"type\":\"click\",\"name\":\"今日歌曲\",\"key\":\"V1001_TODAY_MUSIC\"},{\"name\":\"菜单\",\"sub_button\":[{\"type\":\"view\",\"name\":\"搜索\",\"url\":\"http://www.soso.com/\"},{\"type\":\"miniprogram\",\"name\":\"wxa\",\"url\":\"http://mp.weixin.qq.com\",\"appid\":\"wx286b93c14bbf93aa\",\"pagepath\":\"pages/lunar/index\"},{\"type\":\"click\",\"name\":\"赞一下我们\",\"key\":\"V1001_GOOD\"}]}]}";
                var request = new HttpRequestMessage(HttpMethod.Post, string.Format(apiConfig.menuApi, token));
                request.Content = new StringContent(str);
                var response = _client.SendAsync(request).Result;
                str = response.Content.ReadAsStringAsync().Result;
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
                return str;
            }
            catch (Exception e)
            {

                throw e;
            }
          
        }
    }
}
