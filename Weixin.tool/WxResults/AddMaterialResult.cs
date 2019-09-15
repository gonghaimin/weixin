using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.WxResults
{
    /// <summary>
    /// 新增永久素材，返回实体
    /// </summary>
    public class AddMaterialResult:WxJsonResult
    {
        /// <summary>
        /// 新增的永久素材的media_id
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 新增的图片素材的图片URL（仅新增图片素材时会返回该字段）
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb，主要用于视频与音乐格式的缩略图）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 媒体文件上传时间戳
        /// </summary>
        public int created_at { get; set; }
        /// <summary>
        /// 语音总数量
        /// </summary>
        public int voice_count { get; set; }
        /// <summary>
        /// 视频总数量
        /// </summary>
        public int video_count { get; set; }
        /// <summary>
        /// 	图片总数量
        /// </summary>
        public int image_count { get; set; }
        /// <summary>
        /// 图文总数量
        /// </summary>
        public int news_count { get; set; }
    }
}
