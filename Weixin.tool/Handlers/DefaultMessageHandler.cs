using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Handlers.Factory;
using Weixin.Tool.Messages.Base;
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
            // System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); 运行制定构造函数，可以运行一些对象的静态函数
            this.BuildRequestMessage();
        }

        public override BuildResponseMessage BuildResponseMessage { get; set; }=(s)=>{};

        public override string HandleRequest()
        {
            MsgCryptUtility mc = null;
            if (this.SignModel != null && !string.IsNullOrEmpty(this.SignModel.msg_signature))
            {
                mc = new MsgCryptUtility(WeiXinContext.Config.Token, WeiXinContext.Config.EncodingAESKey, WeiXinContext.Config.AppID);
                var requestXml = this.RequestXml;

                var ret = mc.DecryptMsg(this.SignModel.msg_signature, this.SignModel.timestamp, this.SignModel.nonce, requestXml, ref requestXml);
                if (ret != 0)
                {
                    throw new Exception("消息解密失败");
                }
                this.RequestXml = requestXml;
            }
            var requestMessage=RequestMessageFactory.GetRequestEntity(this.RequestXml);
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<T>(requestMessage);
   
            this.BuildResponseMessage?.Invoke(this.ResponseMessage);

            var msg = ResponseMessageFactory.ConvertEntityToXmlStr(this.ResponseMessage);
            if (mc != null)
            {
                var ret = mc.EncryptMsg(msg, this.SignModel.timestamp, this.SignModel.nonce, ref msg);
                if (ret != 0)
                {
                    throw new Exception("消息加密失败");
                }
            }
            return msg;
        }
    }
}
