using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
using Weixin.Tool.Utility;
using Weixin.WebApi.Extensions;

namespace Weixin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MPController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(string signature, string timestamp, string nonce, string echostr)
        {
            var token = "huayueniansi";
            var encodingAESKey = "abUdAO4KHKBnK9mYE5yyqVOZKzo0jtjzrIbpCQM50k2";
            string[] tmpArr = { "huayueniansi", timestamp, nonce };
            Array.Sort(tmpArr);// 字典排序

            string tmpStr = string.Join("", tmpArr);
            EncryptionService ns = new EncryptionService();
            tmpStr = ns.EncryptText(tmpStr);
            tmpStr = tmpStr.ToLower();
            var result= CheckSignature.Check(signature,timestamp,nonce,token);
            return echostr;
        }
        [HttpPost]
        public ActionResult<string> Post()
        {
            string requestXml = Common.ReadRequest(this.Request);
            IHandler handler = HandlerFactory.CreateHandler(requestXml);
            var res = handler.HandleRequest();
            return Content(res);
        }
    }
}
