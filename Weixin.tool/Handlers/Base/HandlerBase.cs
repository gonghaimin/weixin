using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Factory;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers.Base
{
    public abstract class HandlerBase: IHandler 
    {
        public string RequestXml { get; set; }
        public SignModel SignModel { get; set; }
        public IRequestMessageBase RequestMessage { get; internal set; }
        public IResponseMessageBase ResponseMessage { get; set; }
      
        protected void DecryptMsg()
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
        protected void EncryptMsg(ref string response)
        {
            if (this.SignModel != null && !string.IsNullOrEmpty(this.SignModel.msg_signature))
            {
                MsgCryptUtility mc = new MsgCryptUtility(WeiXinContext.Config.Token, WeiXinContext.Config.EncodingAESKey, WeiXinContext.Config.AppID);
               
                var ret = mc.EncryptMsg(response, this.SignModel.timestamp, this.SignModel.nonce, ref response);
                if (ret != 0)
                {
                    throw new Exception("消息加密失败");
                } 
            }
        }
        public abstract string HandleRequest();
        public virtual IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventViewRequest(RequestMessageEventView requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventUnsubscribeRequest(RequestMessageEventUnsubscribe requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventSubscribeRequest(RequestMessageEventSubscribe requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventScanRequest(RequestMessageEventScan requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventLocationRequest(RequestMessageEventLocation requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase OnEventClickRequest(RequestMessageEventClick requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
        public virtual IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage=ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "对不起，暂时不能处理你的消息，请联系客服！";
            return responseMessage;
        }
    }
}
