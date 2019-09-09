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
    public class ImageHandler : IHandler
    {
        public ImageHandler(string requestXml) : base(requestXml)
        {
        }

        public ImageHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }
        public override string HandleRequest()
        {
            var requestMessage = new RequestMessageImage(this.RequestXml);
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "床前明月光";
            return responseMessage.GetResponse(this.SignModel);
        }
    }
}
