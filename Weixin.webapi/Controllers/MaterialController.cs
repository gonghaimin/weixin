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
    public class MaterialController: ControllerBase
    {
        private readonly MaterialService _materialService;
        public MaterialController( MaterialService materialService)
        {
            _materialService = materialService;
        }
        [HttpGet]
        [Route("/api/Material/Get_materialcount")]
        public ActionResult<AddMaterialResult> Get_materialcount()
        {
            return _materialService.Get_materialcount();
        }
        [HttpPost]
        [Route("/api/Material/Batchget_material")]
        public ActionResult<MaterialListResult> Batchget_material(UploadMediaFileType type, int offset, int count)
        {
            return _materialService.Batchget_material(type, offset, count);
        }
        [HttpPost]
        [Route("/api/Material/Get_material")]
        public ActionResult<Dictionary<string,object>> Get_material(string media_id)
        {
            return _materialService.Get_material(media_id);
        }
    }
}
