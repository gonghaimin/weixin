using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using CommonService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using Weixin.Tool.WxResults;
using Weixin.WebApi.Extensions;

namespace Weixin.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController: ControllerBase
    {
        private readonly MenuService _menuService;
        private readonly MaterialService _materialService;
        public MenuController(MenuService menuService,MaterialService materialService)
        {
            _menuService = menuService;
            _materialService = materialService;
        }
        [HttpGet]
        [Route("/api/Menu/GetMenu")]
        public ActionResult<MenuResult> GetMenu()
        {
            return _menuService.GetMenu();
        }

        [HttpPost]
        [Route("/api/Menu/CreateMenu")]
        public ActionResult<WxJsonResult> CreateMenu()
        {

            return _menuService.CreateMenu();
        }
 
    }
}
