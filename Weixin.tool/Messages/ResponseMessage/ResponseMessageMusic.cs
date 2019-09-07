using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 音乐消息
    /// </summary>
    public class ResponseMessageMusic : ResponseMessageBase
    {
        protected override ResponseMsgType MsgType => ResponseMsgType.Music;
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
        private string Template
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
                             <MsgId>{9}</MsgId>
                        </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType.ToString(), this.Title, this.Description, this.MusicUrl, this.HQMusicUrl, this.ThumbMediaId,this.MsgId);
        }
    }
}
