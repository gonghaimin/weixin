using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;
using HttpUtility = Weixin.Tool.Utility.HttpUtility;

namespace Weixin.Tool
{

    public static class WeiXinContext
    {

        public static WeixinSetting Config ;

        private static IDistributedCache Cache;

        public static IHttpClientFactory ClientFactory;
        private static string  key = "AccessToken";
        public static void RegisterWX(IOptions<WeixinSetting> settings, IServiceProvider serviceProvider)
        {
            if (WeiXinContext.Cache == null)
            {
                WeiXinContext.Cache = serviceProvider.GetRequiredService< IDistributedCache>();
            }
            if (WeiXinContext.ClientFactory == null)
            {
                WeiXinContext.ClientFactory= serviceProvider.GetRequiredService<IHttpClientFactory>();
            }
            if (WeiXinContext.Config == null)
            {
                WeiXinContext.Config = new WeixinSetting();
                WeiXinContext.Config.AppID = settings.Value.AppID;
                WeiXinContext.Config.AppSecret = settings.Value.AppSecret;
                WeiXinContext.Config.EncodingAESKey = settings.Value.EncodingAESKey;
                WeiXinContext.Config.Token = settings.Value.Token;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string AccessToken
        {
            get
            {

                return GetAccessToken(); ;
            }
        }
        public static void ClearAccessToken()
        {
            WeiXinContext.Cache.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            
            var accessToken=WeiXinContext.Cache.GetString(key);
            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WeiXinContext.Config.AppID, WeiXinContext.Config.AppSecret);
            string result = HttpUtility.GetData(url);

            XDocument doc = XmlUtility.ParseJson(result, "root");
            XElement root = doc.Root;
            if (root != null)
            {
                XElement access_token = root.Element("access_token");
                if (access_token != null)
                {
                    var expires_Period = int.Parse(root.Element("expires_in").Value);
                    WeiXinContext.Cache.SetString(key, access_token.Value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow=TimeSpan.FromSeconds(expires_Period) });
                    return access_token.Value;
                }
            }
            return null;
        }


    }
}
