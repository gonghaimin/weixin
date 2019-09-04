﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 视频消息，可用于被动回复用户消息
    /// </summary>
    public class VideoMessage: IReplyMessage
    {
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 视频消息媒体id，可以调用获取临时素材接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }
        public VideoMessage()
        {
            this.MsgType = MsgTypeEnum.video.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static VideoMessage LoadFromXml(string xml)
        {
            VideoMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new VideoMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.ThumbMediaId = element.Element("ThumbMediaId")?.Value;
                    m.MediaId = element.Element(Common.MediaId)?.Value;
                }
            }

            return m;
        }
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
                                   <Video>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                    <Title><![CDATA[{5}]]></Title>
                                    <Description><![CDATA[{6}]]></Description>
                                  </Video>
                            </xml>";
            }
        }
        protected override bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(this.MediaId))
            {
                msg = "MediaId";
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
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.MediaId, this.Title,this.Description);
        }
    }
}