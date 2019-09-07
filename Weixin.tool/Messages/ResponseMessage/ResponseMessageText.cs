using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ResponseMessageText : ResponseMessageBase
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        protected override ResponseMsgType MsgType => ResponseMsgType.Text;
        private string Template
        {
            get
            {
                return @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[{3}]]></MsgType>
                                <Content><![CDATA[{4}]]></Content>
                                <MsgId>{5}</MsgId>
                            </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType.ToString(), this.Content,this.MsgId);
        }
    }
}
