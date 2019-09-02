using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Messages
{
    public enum MsgTypeEnum
    {
        text,
        image,
        voice,
        video,
        shortvideo,
        location,
        link,
        @event,
        music,
        news
    }

    public enum EventEnum
    {
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe,
        /// <summary>
        /// 取消订阅)
        /// </summary>
        unsubscribe,
        /// <summary>
        /// 扫码
        /// </summary>
        SCAN,
        /// <summary>
        /// 点击菜单跳转链接时的事件推送
        /// </summary>
        VIEW,
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        LOCATION
    }
}
