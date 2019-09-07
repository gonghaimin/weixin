using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Weixin.Tool.Enums
{
    //
    // 摘要:
    //     /// 消息响应类型 ///
    public enum ResponseMsgType
    {
        [Description("未知")]
        Unknown = -1,
        [Description("文本")]
        Text = 0,
        [Description("单图文")]
        News = 1,
        [Description("音乐")]
        Music = 2,
        [Description("图片")]
        Image = 3,
        [Description("语音")]
        Voice = 4,
        [Description("视频")]
        Video = 5,
    }
}
