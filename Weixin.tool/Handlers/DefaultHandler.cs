using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class DefaultHandler : IHandler
    {
        public DefaultHandler(string requestXml) : base(requestXml)
        {
        }

        public DefaultHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            var requestMessage = new RequestMessageText(this.RequestXml);
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你发送的消息暂时不能处理";
            return responseMessage.GetResponse(this.SignModel);
        }
    }
}
