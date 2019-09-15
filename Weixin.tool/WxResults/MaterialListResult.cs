using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.WxResults
{
    /// <summary>
    /// 获取素材列表
    /// </summary>
    public class MaterialListResult:WxJsonResult
    {
        /// <summary>
        /// 该类型的素材的总数
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 本次调用获取的素材的数量
        /// </summary>
        public int item_count { get; set; }
        public List<MaterialDetial> item { get; set; }
  
    }
    public class MaterialDetial
    {
        public string media_id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 这篇图文消息素材的最后更新时间
        /// </summary>
        public long update_time { get; set; }
        /// <summary>
        /// 图文页的URL，或者，当获取的列表是图片素材列表时，该字段是图片的URL
        /// </summary>
        public string url { get; set; }
    }
}
