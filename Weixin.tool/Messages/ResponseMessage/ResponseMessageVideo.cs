using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ResponseMessageVideo : ResponseMessageBase
    {
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
        protected override ResponseMsgType MsgType => ResponseMsgType.Video;
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
                                   <Video>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                    <Title><![CDATA[{5}]]></Title>
                                    <Description><![CDATA[{6}]]></Description>
                                  </Video>
                                     <MsgId>{7}</MsgId>
                            </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.MediaId, this.Title, this.Description,this.MsgId);
        }
    }
}
