using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weixin.Core.Data;
using Weixin.Core.Domain;
using Weixin.Data;
using Weixin.Tool;

namespace Weixin.WebApi.Controllers
{
    /// <summary>
    /// Sqlite
    /// https://blog.csdn.net/qq_34759481/article/details/85013025
    /// https://www.cnblogs.com/anech/p/6873385.html 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public WeixinContext _context { get; set; }
        public readonly IRepository<User> Users;
        public ValuesController(ILogger<Program> context, IApplicationBuilder app)
        {
            
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _context.Users.Add(new User() {
                UserName="ghm"
            });
            _context.SaveChanges();
            var token=WeCharBase.AccessToken;
            return new string[] { token };
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
