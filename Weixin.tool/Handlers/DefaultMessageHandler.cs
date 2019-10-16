using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Handlers.Factory;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.MsgHelpers;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class DefaultMessageHandler : HandlerBase, IHandler
    {
     
        public DefaultMessageHandler()
        {

        }
        public override IRequestMessageBase RequestMessage { get; internal set; }
        public override IResponseMessageBase ResponseMessage { get; set; }

        public DefaultMessageHandler(SignModel signModel,string requestXml)
        {
            this.SignModel = signModel;
            this.RequestXml = requestXml;
            try
            {
                this.DecryptMsg();
            }
            catch (Exception ex)
            {
                var val = new RequestMessageBase();
                val.FillEntityWithXml<RequestMessageBase>(XDocument.Parse(requestXml));
                this.RequestMessage = val;
                this.RequestMessage.MsgType = RequestMsgType.text;
                throw ex;
            }
            
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
                case RequestMsgType.voice:
                    this.ResponseMessage = OnVoiceRequest((RequestMessageVoice)this.RequestMessage);
                    break;
                case RequestMsgType.video:
                    this.ResponseMessage = OnVideoRequest((RequestMessageVideo)this.RequestMessage);
                    break;
                case RequestMsgType.shortvideo:
                    this.ResponseMessage = OnShortVideoRequest((RequestMessageShortVideo)this.RequestMessage);
                    break;
                case RequestMsgType.location:
                    this.ResponseMessage = OnLocationRequest((RequestMessageLocation)this.RequestMessage);
                    break;
                case RequestMsgType.link:
                    this.ResponseMessage = OnLinkRequest((RequestMessageLink)this.RequestMessage);
                    break;
                case RequestMsgType.image:
                    this.ResponseMessage = OnImageRequest((RequestMessageImage)this.RequestMessage);
                    break;
                case RequestMsgType.@event:
                    var eventBase = this.RequestMessage as RequestMessageEventBase;
                    switch (eventBase.Event)
                    {
                        case Event.CLICK:
                            this.ResponseMessage = OnEventClickRequest((RequestMessageEventClick)this.RequestMessage);
                            break;
                        case Event.LOCATION:
                            this.ResponseMessage = OnEventLocationRequest((RequestMessageEventLocation)this.RequestMessage);
                            break;
                        case Event.scan:
                            this.ResponseMessage = OnEventScanRequest((RequestMessageEventScan)this.RequestMessage);
                            break;
                        case Event.subscribe:
                            this.ResponseMessage = OnEventSubscribeRequest((RequestMessageEventSubscribe)this.RequestMessage);
                            break;
                        case Event.unsubscribe:
                            this.ResponseMessage = OnEventUnsubscribeRequest((RequestMessageEventUnsubscribe)this.RequestMessage);
                            break;
                        case Event.VIEW:
                            this.ResponseMessage = OnEventViewRequest((RequestMessageEventView)this.RequestMessage);
                            break;
                    }
                    break;
            }
            var response = ResponseMessageFactory.ConvertEntityToXmlStr(this.ResponseMessage);
            EncryptMsg(ref response);
            return response;
        }
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "对不起，暂时不能处理你的消息，请联系客服！";
            return responseMessage;
        }
        public override IResponseMessageBase OnEventClickRequest(RequestMessageEventClick requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你触发自定义菜单事件：" + requestMessage.Event.ToString() + ";EventKey："+requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnEventLocationRequest(RequestMessageEventLocation requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你上报地理位置事件：" + requestMessage.Event.ToString() + ";EventKey：" + requestMessage.EventKey + "位置信息Latitude:" + requestMessage.Latitude+ ";Longitude:" + requestMessage.Longitude+ ";Precision:" + requestMessage.Precision;
            return responseMessage;
        }

        public override IResponseMessageBase OnEventScanRequest(RequestMessageEventScan requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "扫码事件:" + requestMessage.Event.ToString() + ";EventKey：" + requestMessage.EventKey + ";Ticket" + requestMessage.Ticket;
            return responseMessage;
        }

        public override IResponseMessageBase OnEventSubscribeRequest(RequestMessageEventSubscribe requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "关注事件:" + requestMessage.Event.ToString() + ";EventKey：" + requestMessage.EventKey + ";Ticket" + requestMessage.Ticket;
            return responseMessage;
        }

        public override IResponseMessageBase OnEventUnsubscribeRequest(RequestMessageEventUnsubscribe requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "取消关注事件:" + requestMessage.Event.ToString() + ";EventKey：" + requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnEventViewRequest(RequestMessageEventView requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "点击菜单跳转链接时的事件:" + requestMessage.Event.ToString() + ";EventKey：" + requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            responseMessage.Articles = new List<Article>();
            var artcle = new Article();
            artcle.Description = "你发送了图片消息";
            artcle.PicUrl = "http://h5ds-cdn.oss-cn-beijing.aliyuncs.com/upload/53a1bad4-c27b-4e9b-8432-c85d10f2880b.png";
            artcle.Title = "植物";
            artcle.Url = "www.baidu.com";
            responseMessage.Articles.Add(artcle);
            return responseMessage;
        }

        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageMusic>(requestMessage);
            responseMessage.Music = new Music();
            responseMessage.Music.HQMusicUrl = "http://h5ds-cdn.oss-cn-beijing.aliyuncs.com/upload/dfeb140f-4446-4a05-846b-7f71382e9367.mp3";
            responseMessage.Music.MusicUrl = "http://h5ds-cdn.oss-cn-beijing.aliyuncs.com/upload/dfeb140f-4446-4a05-846b-7f71382e9367.mp3";
            return responseMessage;
        }

        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你发送了位置,Label:" + requestMessage.Label+ ";Location_X:" + requestMessage.Location_X+ ";Location_Y:" + requestMessage.Location_Y;
            return responseMessage;
        }

        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你发送了小视频:MediaId=" + requestMessage.MediaId;
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            TemplateService templateService = new TemplateService();
            var result=templateService.templateSend(requestMessage.FromUserName);
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "对不起，暂时不能处理你的消息，请联系客服！";
            return responseMessage;
        }

        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageMusic>(requestMessage);
            responseMessage.Music = new Music();
            responseMessage.Music.HQMusicUrl = "http://h5ds-cdn.oss-cn-beijing.aliyuncs.com/upload/dfeb140f-4446-4a05-846b-7f71382e9367.mp3";
            responseMessage.Music.MusicUrl = "http://h5ds-cdn.oss-cn-beijing.aliyuncs.com/upload/dfeb140f-4446-4a05-846b-7f71382e9367.mp3";
            return responseMessage;
        }

        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = ResponseMessageFactory.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "你发送了语音:MediaId=" + requestMessage.MediaId+ ";Recognition:" + requestMessage.Recognition;
            return responseMessage;
        }
    }
}
