using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Enums
{
    public enum RequestMsgType
    {
        Unknown = -1,
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
    }

}
