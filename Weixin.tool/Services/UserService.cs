using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Services
{
    public  class UserService: IService
    {
        public UserInfoResult GetUserInfo(string openid)
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var result = ApiHandler.GetJson<UserInfoResult>(string.Format(ApiConfig.userinfo, accessToken,openid));
                return result;
            });
        }
    }
}
