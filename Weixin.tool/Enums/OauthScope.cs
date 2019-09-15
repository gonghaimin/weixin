﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Enums
{
    /// <summary>
    /// 网页授权code
    /// </summary>
    public enum OauthScope
    {
        /// <summary>
        /// 不弹出授权页面，直接跳转，只能获取用户openid
        /// </summary>
        snsapi_base,
        /// <summary>
        /// 弹出授权页面，可通过openid拿到昵称、性别、所在地。并且， 即使在未关注的情况下，只要用户授权，也能获取其信息 
        /// </summary>
        snsapi_userinfo
    }
}
