using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Utility
{
    public enum MsgTypeEnum
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        text,
        /// <summary>
        /// 图片消息
        /// </summary>
        image,
        /// <summary>
        /// 语音消息
        /// </summary>
        voice,
        /// <summary>
        /// 视频消息
        /// </summary>
        video,
        /// <summary>
        /// 小视频消息
        /// </summary>
        shortvideo,
        /// <summary>
        /// 位置消息
        /// </summary>
        location,
        /// <summary>
        /// 链接消息
        /// </summary>
        link,
        /// <summary>
        /// 事件消息
        /// </summary>
        @event,
        /// <summary>
        /// 音乐消息
        /// </summary>
        music,
        /// <summary>
        /// 图文消息
        /// </summary>
        news
    }

    public enum EventEnum
    {
        /// <summary>
        /// 订阅 和  用户未关注时，进行关注后的事件推送
        /// </summary>
        subscribe,
        /// <summary>
        /// 取消订阅)
        /// </summary>
        unsubscribe,
        /// <summary>
        /// 扫码,用户已关注时的事件推送
        /// </summary>
        SCAN,
        /// <summary>
        /// 点击菜单跳转链接时的事件推送
        /// </summary>
        VIEW,
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        LOCATION,
        /// <summary>
        /// 点击菜单拉取消息时的事件推送
        /// </summary>
        CLICK
    }
}
