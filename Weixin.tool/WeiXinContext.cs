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

        private static IHttpClientFactory ClientFactory;
        public static HttpClient Client
        {
            get
            {
               return ClientFactory.CreateClient("weixin");
            }
        }

        private static string  key = "AccessToken";
        public static void RegisterWX(IOptions<WeixinSetting> settings, IServiceProvider serviceProvider)
        {
            if (Cache == null)
            {
                Cache = serviceProvider.GetRequiredService< IDistributedCache>();
            }
            if (ClientFactory == null)
            {
                ClientFactory= serviceProvider.GetRequiredService<IHttpClientFactory>();
            }
            if (Config == null)
            {
                Config = new WeixinSetting();
                Config.AppID = settings.Value.AppID;
                Config.AppSecret = settings.Value.AppSecret;
                Config.EncodingAESKey = settings.Value.EncodingAESKey;
                Config.Token = settings.Value.Token;
                Config.Oauth_redirect_uri = settings.Value.Oauth_redirect_uri;
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
            Cache.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            
            var accessToken=Cache.GetString(key);
            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", Config.AppID, Config.AppSecret);
            string result = HttpUtility.GetData(url);

            XDocument doc = XmlUtility.ParseJson(result, "root");
            XElement root = doc.Root;
            if (root != null)
            {
                XElement access_token = root.Element("access_token");
                if (access_token != null)
                {
                    var expires_Period = int.Parse(root.Element("expires_in").Value);
                    Cache.SetString(key, access_token.Value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow=TimeSpan.FromSeconds(expires_Period) });
                    return access_token.Value;
                }
            }
            return null;
        }


    }
}
