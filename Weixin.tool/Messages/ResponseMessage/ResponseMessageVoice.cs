using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    public class ResponseMessageVoice : ResponseMessageBase
    {
        protected override ResponseMsgType MsgType => ResponseMsgType.Voice;
        /// <summary>
        /// 语音消息媒体id，可以调用获取临时素材接口拉取该媒体
        /// </summary>
        public string MediaId { get; set; }

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
                                  <Voice>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                   </Voice>
                                    <MsgId>{5}</MsgId>
                            </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.MediaId,this.MsgId);
        }
    }
}
