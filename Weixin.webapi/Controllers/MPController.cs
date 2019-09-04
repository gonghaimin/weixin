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
            if (CheckSignature.Check(signature, timestamp, nonce, Common.Token))
            {
                return echostr;
            }
            return string.Empty;
        }
        [HttpPost]
        public ActionResult<string> Post([FromQuery]SignModel model)
        {
            if (CheckSignature.Check(model.signature, model.timestamp, model.nonce, Common.Token))
            {
                string requestXml = Common.ReadRequest(this.Request);
                IHandler handler = HandlerFactory.CreateHandler(requestXml, model);
                var res = handler.HandleRequest();
                return Content(res);
            }
            return null;
        }
    }
   
}
