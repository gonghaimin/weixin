using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Factory;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers.Base
{
    public delegate void BuildResponseMessage(IResponseMessageBase responseMessage);
    public abstract class HandlerBase: IHandler 
    {
        public string RequestXml { get; set; }
        public SignModel SignModel { get; set; }
        public IRequestMessageBase RequestMessage { get; internal set; }
        protected void BuildRequestMessage()
        {
            if (this.SignModel != null && !string.IsNullOrEmpty(this.SignModel.msg_signature))
            {
                MsgCryptUtility mc = new MsgCryptUtility(WeiXinContext.Config.Token, WeiXinContext.Config.EncodingAESKey, WeiXinContext.Config.AppID);
                var requestXml = this.RequestXml;

                var ret = mc.DecryptMsg(this.SignModel.msg_signature, this.SignModel.timestamp, this.SignModel.nonce, requestXml, ref requestXml);
                if (ret != 0)
                {
                    throw new Exception("消息解密失败");
                }
                this.RequestXml = requestXml;
            }
            this.RequestMessage = RequestMessageFactory.GetRequestEntity(this.RequestXml);
        }
        public abstract BuildResponseMessage BuildResponseMessage { get; set; }
        public abstract string HandleRequest();
        public abstract IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage);
        //
        // 摘要:
        //     /// 默认返回消息（当任何OnXX消息没有被重写，都将自动返回此默认消息） ///
        public abstract IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage);
    }
}
