using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 音乐消息，用于被动回复
    /// </summary>
    public class MusicMessage:  IReplyMessage
    {
        public MusicMessage()
        {
            this.MsgType = MsgTypeEnum.music.ToString();
        }
        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 缩略图的媒体id，通过素材管理中的接口上传多媒体文件，得到的id
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string Template
        {
            get
            {
                return @"<xml>
                          <ToUserName><![CDATA[{0}]]></ToUserName>
                          <FromUserName><![CDATA[{1}]]></FromUserName>
                          <CreateTime>{2}</CreateTime>
                          <MsgType><![CDATA[{3}]]></MsgType>
                          <Music>
                            <Title><![CDATA[{4}]]></Title>
                            <Description><![CDATA[{5}]]></Description>
                            <MusicUrl><![CDATA[{6}]]></MusicUrl>
                            <HQMusicUrl><![CDATA[{7}]]></HQMusicUrl>
                            <ThumbMediaId><![CDATA[{8}]]></ThumbMediaId>
                          </Music>
                        </xml>";
            }
        }
        protected override bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(this.ThumbMediaId))
            {
                msg = "ThumbMediaId";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 生成回复内容
        /// </summary>
        /// <returns></returns>
        protected override string GenerateContent()
        {
            this.CreateTime = Common.GetNowTime();
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.Title, this.Description,this.MusicUrl,this.HQMusicUrl,this.ThumbMediaId);
        }
    }
}
