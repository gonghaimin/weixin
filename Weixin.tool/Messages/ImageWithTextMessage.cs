using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class ImageWithTextMessage : Message
    {
        public ImageWithTextMessage()
        {
            this.MsgType = MsgTypeEnum.news.ToString();
        }
        /// <summary>
        /// 音乐标题
        /// </summary>
        public int ArticleCount { get; set; } = 1;
        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Articles { get; set; }
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 缩略图的媒体id，通过素材管理中的接口上传多媒体文件，得到的id
        /// </summary>
        public string PicUrl { get; set; }

        public string Url { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        public override string Template
        {
            get
            {
                return @"<xml>
                          <ToUserName><![CDATA[{0}]]></ToUserName>
                          <FromUserName><![CDATA[{1}]]></FromUserName>
                          <CreateTime>{2}</CreateTime>
                          <MsgType><![CDATA[{3}]]></MsgType>
                          <ArticleCount>{4}</ArticleCount>
                          <Articles>
                            <item>
                              <Title><![CDATA[{5}]]></Title>
                              <Description><![CDATA[{6}]]></Description>
                              <PicUrl><![CDATA[{7}]]></PicUrl>
                              <Url><![CDATA[{8}]]></Url>
                            </item>
                          </Articles>
                        </xml>";
            }
        }
        /// <summary>
        /// 生成内容
        /// </summary>
        /// <returns></returns>
        public override string GenerateContent()
        {
            this.CreateTime = Common.GetNowTime();
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.ArticleCount, this.Title, this.Description, this.PicUrl, this.Url);
        }
    }
}
