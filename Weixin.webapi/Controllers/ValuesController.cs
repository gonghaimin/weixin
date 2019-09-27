using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
            var token = WeiXinContext.AccessToken;
            return new string[] { token, AppsettingsUtility.GetSetting("Logging:LogLevel:Default") };
        }

        [HttpGet]
        [Route("/api/values/createMenu")]
        public ActionResult<string> createMenu()
        {
            return null;
        }

        [HttpGet]
        [Route("/api/values/auth")]
        public ActionResult auth()
        {
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
            var phoneType = "";
            var agent = HttpContext.Request.Headers["User-Agent"][0];
            foreach (string item in keywords)
            {
                if (agent.Contains(item)){
                    phoneType = item;
                    break;
                }
            }
            if(phoneType == "iPhone" || phoneType == "iPod" || phoneType == "iPad")
            {
                return Redirect("https://itunes.apple.com/cn/app/google-authenticator/id388497605");
            }
            else
            {
                return Redirect("https://a.app.qq.com/o/simple.jsp?pkgname=com.google.android.apps.authenticator2");
            }
        }
    }
}
