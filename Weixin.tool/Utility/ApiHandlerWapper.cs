using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Utility
{
    //
    // 摘要:
    //     /// 针对AccessToken无效或过期的自动处理类 ///
    public static class ApiHandlerWapper
    {
        /// <summary>
        /// 使用AccessToken进行操作时，如果遇到AccessToken错误的情况，重新获取AccessToken一次，并重试。 /// 使用此方法之前必须在startup注册  WeiXinContext.RegisterWX
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static T TryCommonApi<T>(Func<string, T> fun, bool retryIfFaild = true) where T : WxJsonResult, new()
        {
            var access_token = WeiXinContext.AccessToken;
            var result = fun(access_token);
            if (result.ReturnCode == ReturnCode.请求成功)
            {
                return result;
            }
            if (result.ReturnCode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
            {
                if (retryIfFaild)
                {
                    WeiXinContext.ClearAccessToken();
                    return TryCommonApi<T>(fun, false);
                }
            }
            throw new Exception(result.ToString());
        }

        public static string Get(string url, int timeOut, Action<string, string> afterReturnText)
        {
            HttpClient client = WeiXinContext.ClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Timeout", timeOut.ToString());
            client.DefaultRequestHeaders.Add("KeepAlive", "true");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.SendAsync(request).Result;
            var text = response.Content.ReadAsStringAsync().Result;
            afterReturnText?.Invoke(url, text);
            return text;
        }

        public static T GetJson<T>(string url, int timeOut = 1000, Action<string, string> afterReturnText = null)
        {
            var text = Get(url, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static Dictionary<string, object> GetDict(string url, int timeOut = 1000, Action<string, string> afterReturnText = null)
        {
            var text = Get(url, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        }
        public static string Post(string url, HttpContent content, int timeOut = 1000, Action<string, string, string> afterReturnText = null)
        {
            HttpClient client = WeiXinContext.ClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Timeout", timeOut.ToString());
            client.DefaultRequestHeaders.Add("KeepAlive", "true");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            var response = client.SendAsync(request).Result;
            var text = response.Content.ReadAsStringAsync().Result;
            var param = content.ReadAsStringAsync().Result;
            afterReturnText?.Invoke(url, param, text);
            return text;
        }
        public static T PostGetJson<T>(string url, HttpContent content, int timeOut = 1000, Action<string, string, string> afterReturnText = null)
        {
            var text = Post(url, content, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static Dictionary<string, object> PostDict(string url, HttpContent content, int timeOut = 1000, Action<string, string, string> afterReturnText = null)
        {
            var text = Post(url, content, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        }
    }
}
