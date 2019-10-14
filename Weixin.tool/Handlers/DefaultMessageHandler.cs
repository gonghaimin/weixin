using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Handlers.Factory;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class DefaultMessageHandler : HandlerBase, IHandler
    {
     
        public DefaultMessageHandler()
        {

        }
        public DefaultMessageHandler(SignModel signModel,string requestXml)
        {
            this.SignModel = signModel;
            this.RequestXml = requestXml;
            this.DecryptMsg();
            // System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); 运行制定构造函数，可以运行一些对象的静态函数
        }

        public override string HandleRequest()
        {
            var requestMsgType = this.RequestMessage.MsgType;
            switch (requestMsgType)
            {
                case RequestMsgType.text:
                    this.ResponseMessage = OnTextRequest((RequestMessageText)this.RequestMessage);
                    break;
            }
            var response = ResponseMessageFactory.ConvertEntityToXmlStr(this.ResponseMessage);
            EncryptMsg(ref response);
            return response;
        }
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventClickRequest(RequestMessageEventClick requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventLocationRequest(RequestMessageEventLocation requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventScanRequest(RequestMessageEventScan requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventSubscribeRequest(RequestMessageEventSubscribe requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventUnsubscribeRequest(RequestMessageEventUnsubscribe requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnEventViewRequest(RequestMessageEventView requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "对不起，暂时不能处理你的消息，请联系客服！";
            return responseMessage;
        }

        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            throw new NotImplementedException();
        }

        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            throw new NotImplementedException();
        }
    }
}
