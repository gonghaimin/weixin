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
using Weixin.Tool.Enums;
using Weixin.Tool.Handlers;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages.ResponseMessage;
using Weixin.Tool.Models;
using Weixin.Tool.Services;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;
using Weixin.WebApi.Extensions;
namespace Weixin.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomController:ControllerBase
    {
        private readonly CustomService _customService;
        public CustomController(CustomService customService)
        {
            _customService = customService;
        }

        [HttpPost]
        [Route("/api/Custom/CustomSendText")]
        public ActionResult<Dictionary<string, object>> CustomSendText(CustomSend send)
        {
            return _customService.CustomSendText(send);
        }
    }
}
