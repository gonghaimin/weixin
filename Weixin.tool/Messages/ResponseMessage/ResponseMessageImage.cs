using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ResponseMessageImage : ResponseMessageBase
    {
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        protected string MediaId { get; set; }

        protected override ResponseMsgType MsgType => ResponseMsgType.Image;

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
                                   <Image>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                  </Image>
                                   <MsgId>{5}</MsgId> 
                            </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType.ToString(), this.MediaId,this.MsgId);
        }
    }
}
