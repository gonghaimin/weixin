﻿using System;
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
            string[] tmpArr = { "huayueniansi", timestamp, nonce };
            Array.Sort(tmpArr);// 字典排序

            string tmpStr = string.Join("", tmpArr);
            EncryptionService ns = new EncryptionService();
            tmpStr = ns.EncryptText(tmpStr);
            tmpStr = tmpStr.ToLower();
            return echostr;
            //if (tmpStr == signature && !string.IsNullOrWhiteSpace(echostr))
            //    return echostr;
            //return "";
        }
        [HttpPost]
        public ActionResult<string> Post()
        {
            wxmessage wx = new wxmessage();
            var reader = new StreamReader(Request.Body);
            var contentFromBody = reader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(contentFromBody);
            //xml.Load(contentFromBody);
            
            return Content(contentFromBody);
                 
           
            //if (tmpStr == signature && !string.IsNullOrWhiteSpace(echostr))
            //    return echostr;
            //return "";
        }
    }
    public class wxmessage
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string Content { get; set; }
        public string EventKey { get; set; }
    }
}
