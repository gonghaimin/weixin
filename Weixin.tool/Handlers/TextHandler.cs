﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.RequestMessage;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    /// <summary>
    /// 文本信息处理类
    /// </summary>
    public class TextHandler : IHandler
    {
        private readonly UserService _userService = new UserService();
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        public override string HandleRequest()
        {
            string response = string.Empty;
            var requestMessage = new RequestMessageText(this.RequestXml);
            string Content = requestMessage.Content;
            if (string.IsNullOrEmpty(Content))
            {
                response = "您什么都没输入，没法帮您啊，%>_<%。";
            }
            else
            {
                response = HandleOther(Content);
            }
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = response;

            var user=_userService.GetUserInfo(requestMessage.FromUserName);
            responseMessage.Content = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            return responseMessage.GetResponse(this.SignModel);
        }
        /// <summary>
        /// 处理其他消息
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        private string HandleOther(string requestContent)
        {
            string response = string.Empty;
            if (requestContent.Contains("你好") || requestContent.Contains("您好"))
            {
                response = "您也好~";
            }
            else if (requestContent.Contains("傻"))
            {
                response = "我不傻！哼~ ";
            }
            else if (requestContent.Contains("逼") || requestContent.Contains("操"))
            {
                response = "哼，你说脏话！ ";
            }
            else if (requestContent.Contains("是谁"))
            {
                response = "我是大哥大，有什么能帮您的吗？~";
            }
            else if (requestContent.Contains("再见"))
            {
                response = "再见！";
            }
            else if (requestContent.Contains("bye"))
            {
                response = "Bye！";
            }
            else if (requestContent.Contains("谢谢"))
            {
                response = "不客气！嘿嘿";
            }
            else if (requestContent == "h" || requestContent == "H" || requestContent.Contains("帮助"))
            {
                response = @"查询天气，输入tq 城市名称\拼音\首字母";
            }
            else
            {
                response = "您说的，可惜，我没明白啊，试试其他关键字吧。";
            }

            return response;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             