using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Services
{
    /// <summary>
    /// 网页授权
    /// </summary>
    public class OAuthService: IService
    {
        private static string oauth_Access_token = "oauth_Access_token_{0}";
        private static string oauth_refresh_token = "oauth_refresh_token_{0}";
        private static IDistributedCache Cache;
        public OAuthService(IDistributedCache cache)
        {
            Cache = cache;   
        }
        /// <summary>
        /// 网页授权地址
        /// </summary>
        public string OAuthUrl
        {
            get
            {
                return string.Format(ApiConfig.oauth2authorize, WeiXinContext.Config.AppID, WebUtility.UrlEncode(WeiXinContext.Config.Oauth_redirect_uri), OauthScope.snsapi_userinfo);
            }
        }
        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OauthAccessTokenResult GetAccess_token(string code)
        {
            var token = ApiHandler.GetJson<OauthAccessTokenResult>(string.Format(ApiConfig.oauth2access_token, WeiXinContext.Config.AppID, WeiXinContext.Config.AppSecret, code));

            Cache.SetString(string.Format(oauth_Access_token,token.openid), token.access_token,new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow=TimeSpan.FromSeconds(token.expires_in) });

            Cache.SetString(string.Format(oauth_refresh_token,token.openid), token.refresh_token, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) });

            return token;
        }
        /// <summary>
        /// 刷新access_token（如果需要）
        /// </summary>
        /// <returns></returns>
        private string Refresh_token(string openid)
        {
            OauthAccessTokenResult token =null;
            var refresh_token = Cache.GetString(string.Format(oauth_refresh_token, openid));
            if (!string.IsNullOrEmpty(refresh_token))
            {
                token = ApiHandler.GetJson<OauthAccessTokenResult>(string.Format(ApiConfig.oauth2refresh_token, WeiXinContext.Config.AppID, refresh_token));
                Cache.SetString(string.Format(oauth_Access_token, token.openid), token.access_token, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.expires_in) });

                Cache.SetString(string.Format(oauth_refresh_token, token.openid), token.refresh_token, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) });
                return token.access_token;
            }
            return string.Empty;
        }
        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public OauthUserInfoResult GetOauthUserInfo(string openid)
        {
            var access_token = Cache.GetString(string.Format(oauth_Access_token, openid));
            if (string.IsNullOrEmpty(access_token))
            {
                access_token=Refresh_token(openid);
            }
            if (!string.IsNullOrEmpty(access_token))
            {
                var reslut = ApiHandler.GetJson<OauthUserInfoResult>(string.Format(ApiConfig.oauth2userinfo, access_token, openid));
                return reslut;
            }
            else
            {
                throw new Exception(ReturnCode.不合法的access_token.ToString());
            }
        }
    }
}
