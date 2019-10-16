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
        public abstract IRequestMessageBase RequestMessage { get; internal set; }
        public abstract IResponseMessageBase ResponseMessage { get; set; }
        protected virtual void DecryptMsg()
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
        protected virtual void EncryptMsg(ref string response)
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
        public abstract IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage);
        public abstract IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage);
        public abstract IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage);
        public abstract IResponseMessageBase OnTextRequest(RequestMessageText requestMessage);
        public abstract IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage);
        public abstract IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage);
        public abstract IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage);
        public abstract IResponseMessageBase OnEventViewRequest(RequestMessageEventView requestMessage);
        public abstract IResponseMessageBase OnEventUnsubscribeRequest(RequestMessageEventUnsubscribe requestMessage);
        public abstract IResponseMessageBase OnEventSubscribeRequest(RequestMessageEventSubscribe requestMessage);
        public abstract IResponseMessageBase OnEventScanRequest(RequestMessageEventScan requestMessage);
        public abstract IResponseMessageBase OnEventLocationRequest(RequestMessageEventLocation requestMessage);
        public abstract IResponseMessageBase OnEventClickRequest(RequestMessageEventClick requestMessage);
        public abstract IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage);
    }
}
