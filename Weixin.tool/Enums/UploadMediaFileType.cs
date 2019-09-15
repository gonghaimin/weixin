using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Enums
{
    public enum UploadMediaFileType
    {
        //
        // 摘要:
        //     /// 图片: 128K，支持JPG格式 ///
        image,
        //
        // 摘要:
        //     /// 语音：256K，播放长度不超过60s，支持AMR\MP3格式 ///
        voice,
        //
        // 摘要:
        //     /// 视频：1MB，支持MP4格式 ///
        video,
        //
        // 摘要:
        //     /// thumb：64KB，支持JPG格式 ///
        thumb,
        //
        // 摘要:
        //     /// 图文消息 ///
        news
    }
}
