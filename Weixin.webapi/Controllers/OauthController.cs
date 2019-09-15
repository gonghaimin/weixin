using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using CommonService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Weixin.Core.Domain;
using Weixin.Core.Options;
using Weixin.Data;
using Weixin.Tool;
using Weixin.Tool.Handlers;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;
using Weixin.WebApi.Extensions;

namespace Weixin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OauthController: ControllerBase
    {
        private static IDistributedCache _cache;
        private OAuthService _authService;
        public OauthController(OAuthService authService, IDistributedCache cache)
        {
            _authService = authService;
            _cache = cache;
        }
        [HttpGet]
        [Route("/api/Oauth/GetOAuthUrl")]
        public ActionResult<string> GetOAuthUrl()
        {
            return _authService.OAuthUrl;
        }
        [HttpGet]
        [Route("/api/Oauth/OauthCallback")]
        public ActionResult OauthCallback(string code,string state)
        {
            var token = _authService.GetAccess_token(code);
            var user= _authService.GetOauthUserInfo(token.openid);
            var users = new List<OauthUserInfoResult>();
            var str=_cache.GetString("users");
            if (!string.IsNullOrEmpty(str))
            {
                users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OauthUserInfoResult>>(str);
            }
            users.Add(user);
            _cache.SetString("users", Newtonsoft.Json.JsonConvert.SerializeObject(users), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
            return Redirect("http://www.baidu.com/");
        }
    }
}
