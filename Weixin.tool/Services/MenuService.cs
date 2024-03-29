﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Services
{
    public class MenuService: IService
    {
        public  MenuResult  GetMenu()
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var result = ApiHandler.GetJson<MenuResult >(string.Format(ApiConfig.menuget,accessToken));
                return result;
            });
        }

        public WxJsonResult CreateMenu()
        {
            var menus = "{\"button\":[{\"name\":\"发图\",\"sub_button\":[{\"type\":\"pic_sysphoto\",\"name\":\"系统拍照发图\",\"key\":\"rselfmenu_1_0\",\"sub_button\":[]},{\"type\":\"pic_photo_or_album\",\"name\":\"拍照或者相册发图\",\"key\":\"rselfmenu_1_1\",\"sub_button\":[]},{\"type\":\"pic_weixin\",\"name\":\"微信相册发图\",\"key\":\"rselfmenu_1_2\",\"sub_button\":[]},{\"name\":\"发送位置\",\"type\":\"location_select\",\"key\":\"rselfmenu_2_0\"}]},{\"name\":\"扫码\",\"sub_button\":[{\"type\":\"scancode_waitmsg\",\"name\":\"扫码带提示\",\"key\":\"rselfmenu_0_0\",\"sub_button\":[]},{\"type\":\"scancode_push\",\"name\":\"扫码推事件\",\"key\":\"rselfmenu_0_1\",\"sub_button\":[]},{\"type\":\"media_id\",\"name\":\"图片\",\"media_id\":\"LtDcFBnEmWIY1ePpIARkbMty-9w0QAl_gL8t7PHl9_4\"}]},{\"name\":\"菜单\",\"sub_button\":[{\"type\":\"view\",\"name\":\"搜索\",\"url\":\"http://www.soso.com/\"},{\"type\":\"miniprogram\",\"name\":\"wxa\",\"url\":\"http://mp.weixin.qq.com\",\"appid\":\"wx286b93c14bbf93aa\",\"pagepath\":\"pages/lunar/index\"},{\"type\":\"click\",\"name\":\"赞一下我们\",\"key\":\"V1001_GOOD\"}]}]}";
            //{\"type\":\"view_limited\",\"name\":\"图文消息\",\"media_id\":\"MEDIA_ID2\"}
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                HttpContent content = new StringContent(menus);
                var result = ApiHandler.PostGetJson<WxJsonResult>(string.Format(ApiConfig.menucreate,accessToken), content);
                return result;
            });
        }
    }
}
