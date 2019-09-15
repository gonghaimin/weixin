using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;
using Newtonsoft.Json;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Handlers
{
    public class ImageHandler : IHandler
    {
        public override string HandleRequest()
        {
            var requestMessage = new RequestMessageImage(this.RequestXml);
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            MaterialService materialService = new MaterialService();
            var res=materialService.AddMaterial(UploadMediaFileType.image,requestMessage.PicUrl);
            responseMessage.Content =JsonConvert.SerializeObject(res) ;
            return responseMessage.GetResponse(this.SignModel);
        }
    }
}
