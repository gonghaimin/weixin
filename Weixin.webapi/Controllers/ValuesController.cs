using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Weixin.Core.Domain;
using Weixin.Core.Options;
using Weixin.Data;
using Weixin.Tool;
using Weixin.Tool.Utility;
using Weixin.WebApi.Extensions;


namespace Weixin.WebApi.Controllers
{

    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Policy = "common")]
    public class ValuesController : ControllerBase
    {
        private readonly IBaseRepository<User> Users;
        private IOptions<MyOwnModel> settings;

        public ValuesController(IBaseRepository<User> users, IOptions<MyOwnModel> settings, IOptionsSnapshot<MyOwnModel> namedOptionsAccessor)
        {
            this.Users = users;
            this.settings = settings;
            var own = namedOptionsAccessor.Get("自定义配置");
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            HttpContext.Session.SetString("test", "ghm");//存储在IDistributedCache
            var a = Users.Context;
            //Users.Insert(new User() { UserName = "ghm" });
            var menus = CommonApi.GetMenu();
            var token = WeiXinContext.AccessToken;
            return new string[] { token, AppsettingsUtility.GetSetting("Logging:LogLevel:Default") };
        }

        [HttpGet]
        [Route("/api/values/createMenu")]
        public ActionResult<string> createMenu()
        {
            return null;
        }
    }
}
