using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Enums
{
    //
    // 摘要:
    //     /// 当RequestMsgType类型为Event时，Event属性的类型 ///
    public enum Event
    {
        Unknown = -1,
        //
        // 摘要:
        //     /// 地理位置（似乎已从官方API中移除） ///
        LOCATION,
        //
        // 摘要:
        //     /// 订阅 ///
        subscribe,
        //
        // 摘要:
        //     /// 取消订阅 ///
        unsubscribe,
        //
        // 摘要:
        //     /// 自定义菜单点击事件 ///
        CLICK,
        //
        // 摘要:
        //     /// 二维码扫描 ///
        scan,
        //
        // 摘要:
        //     /// URL跳转 ///
        VIEW,
        //
        // 摘要:
        //     /// 弹出系统拍照发图 ///
        pic_sysphoto,
        //
        // 摘要:
        //     /// 弹出拍照或者相册发图 ///
        pic_photo_or_album,
        //
        // 摘要:
        //     /// 弹出微信相册发图器 ///
        pic_weixin,
        //
        // 摘要:
        //     /// 弹出地理位置选择器 ///
        location_select,
        view_miniprogram
    }
}
