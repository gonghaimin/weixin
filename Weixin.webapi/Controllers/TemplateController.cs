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
    public class TemplateController:ControllerBase
    {
        private readonly TemplateService _templateService;
        public TemplateController(TemplateService templateService)
        {
            _templateService = templateService;
        }
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Template/Get_all_private_template")]
        public Dictionary<string, object> Get_all_private_template()
        {
            return _templateService.get_all_private_template();
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/Template/templateSend")]
        public Dictionary<string, object> TemplateSend()
        {
            return _templateService.templateSend();
        }
    }
}
