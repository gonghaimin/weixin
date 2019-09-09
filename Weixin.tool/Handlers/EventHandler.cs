using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class EventHandler : IHandler
    {
        public EventHandler(string requestXml) : base(requestXml)
        {
        }

        public EventHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        public override string HandleRequest()
        {
            string response = string.Empty;
            XElement element = XElement.Parse(this.RequestXml);
            var eventType = element.Element("Event").Value.StringConvertToEnum<Event>();
            RequestMessageBase requestMessage;
            switch (eventType)
            {
                case Event.subscribe:
                    var eventKey = element.Element("EventKey")?.Value;
                    if (string.IsNullOrEmpty(eventKey))
                    {
                        requestMessage = new RequestMessageEventSubscribe(this.RequestXml);
                    }
                    else
                    {
                        requestMessage = new RequestMessageEventQrsceneSubscribe(this.RequestXml);
                    }
                    response = SubscribeEventHandler(requestMessage);
                    break;
                case Event.CLICK:
                    response = ClickEventHandler(new RequestMessageEventClick(this.RequestXml));
                    break;
            }
            return response;
        }

        /// <summary>
        /// 用户关注
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        private string SubscribeEventHandler(RequestMessageBase requestMessage)
		{
            //回复欢迎消息
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "欢迎您关注万睿楼宇自控，我是服务小二，有事就问我～\n\n";
			return responseMessage.GetResponse(this.SignModel);
		}
		
		/// <summary>
		/// 处理点击事件
		/// </summary>
		/// <param name="em"></param>
		/// <returns></returns>
		private string ClickEventHandler(RequestMessageEventClick requestMessage)
		{
			string result = string.Empty;
			if (requestMessage != null && requestMessage.EventKey != null)
			{
				switch (requestMessage.EventKey.ToUpper())
				{
					case "BTN_GOOD":
						result = btnGoodClick(requestMessage);
						break;
					case "BTN_HELP":
						result = btnHelpClick(requestMessage);
						break;
				}
			}
			return result;
		}

		/// <summary>
		/// 赞一下
		/// </summary>
		/// <param name="em"></param>
		/// <returns></returns>
		private string btnGoodClick(RequestMessageBase requestMessage)
		{
            //回复欢迎消息
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = @"谢谢您的支持！";
            return responseMessage.GetResponse(this.SignModel);
        }
		/// <summary>
		/// 帮助
		/// </summary>
		/// <param name="em"></param>
		/// <returns></returns>
		private string btnHelpClick(RequestMessageBase requestMessage)
		{
            //回复欢迎消息
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = @"有事找警察～";
            return responseMessage.GetResponse(this.SignModel);
        }
      
    }
}
