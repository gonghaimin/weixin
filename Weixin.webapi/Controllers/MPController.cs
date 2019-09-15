using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using CommonService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Weixin.Core.Domain;
using Weixin.Core.Options;
using Weixin.Data;
using Weixin.Tool;
using Weixin.Tool.Handlers;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;
using Weixin.WebApi.Extensions;

namespace Weixin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MPController : ControllerBase
    {
        private readonly HandlerFactory _handlerFactory;
        private readonly UserService _userService;
        public MPController(HandlerFactory handlerFactory,UserService userService)
        {
            _handlerFactory = handlerFactory;
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<string> Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, WeiXinContext.Config.Token))
            {
                return echostr;
            }
            return string.Empty;
        }
        [HttpPost]
        public ActionResult<string> Post([FromQuery]SignModel signModel)
        {
            var res = string.Empty;
            var error = string.Empty;
            string requestXml = Common.ReadRequest(this.Request);
            if (signModel != null &&!string.IsNullOrEmpty(signModel.signature)&& !CheckSignature.Check(signModel.signature, signModel.timestamp, signModel.nonce, WeiXinContext.Config.Token))
            {
                error = "验签失败";
            }
            else
            {
                try
                {
                    var handler = _handlerFactory.CreateHandler(requestXml, signModel);
                    res = handler.HandleRequest();
                    return Content(res,Request.ContentType, Encoding.UTF8);
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
            }
            var defaultHandler = new DefaultHandler();
            defaultHandler.RequestXml = requestXml;
            defaultHandler.SignModel = signModel;
            res = defaultHandler.HandleRequest(new ResponseMessageText() { Content= error });
            
            return Content(res, Request.ContentType, Encoding.UTF8);
        }
    }
   
}
