using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Weixin.Core.Data;
using Weixin.Core.Domain;
using Weixin.Core.Options;
using Weixin.Data;
using Weixin.Tool;
using Weixin.WebApi.Extensions;


namespace Weixin.WebApi.Controllers
{

    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "common")]
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
            var a = Users.Context;
            Users.Insert(new User() { UserName = "ghm" });
            var token = WeCharBase.AccessToken;
            return new string[] { token, AppsettingsUtility.GetSetting("Logging:LogLevel:Default") };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

   
}
