﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{
    public class MenuFull_RootButton
    {
        /// <summary>
        /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
        /// </summary>
        public string type
        {
            get;
            set;
        }
        /// <summary>
        /// click等点击类型必须	菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key
        {
            get;
            set;
        }
        /// <summary>
        /// 	菜单标题，不超过16个字节，子菜单不超过60个字节
        /// </summary>
        public string name
        {
            get;
            set;
        }
        /// <summary>
        /// view、miniprogram类型必须	网页 链接，用户点击菜单可打开链接，不超过1024字节。 type为miniprogram时，不支持小程序的老版本客户端将打开本url。
        /// </summary>
        public string url
        {
            get;
            set;
        }
        /// <summary>
        /// miniprogram类型必须	小程序的appid（仅认证公众号可配置）
        /// </summary>
        public string appid
        {
            get;
            set;
        }
        /// <summary>
        /// 	miniprogram类型必须	小程序的页面路径
        /// </summary>
        public string pagepath
        {
            get;
            set;
        }
        /// <summary>
        /// media_id类型和view_limited类型必须	调用新增永久素材接口返回的合法media_id
        /// </summary>
        public string media_id
        {
            get;
            set;
        }
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        public List<MenuFull_RootButton> sub_button
        {
            get;
            set;
        }
    }
}
