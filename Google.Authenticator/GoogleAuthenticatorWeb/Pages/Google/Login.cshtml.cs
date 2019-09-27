using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GoogleAuthenticatorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace GoogleAuthenticatorWeb.Pages.Google
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IDistributedCache _db;
        public LoginModel(ILogger<LoginModel> logger, IDistributedCache cache)
        {
            _logger = logger;
            _db = cache;
        }
        [BindProperty]
        public new LoginUser User { get; set; }
        public IActionResult OnGet()
        {
            var str = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(str))
            {
                return RedirectToPage("/Google/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var key = Util.Md5EncryptStr32(User.Name + User.PassWord);
            var userStr = await _db.GetStringAsync(key);
            if (!string.IsNullOrEmpty(userStr))
            {
                //老用户
                User = JsonConvert.DeserializeObject<LoginUser>(userStr);
            }
            else
            {
                //新用户
                userStr = JsonConvert.SerializeObject(User);
                _db.SetString(key, userStr);
            }
            if (string.IsNullOrEmpty(User.SecretKey))
            {
                //未绑定谷歌验证器
                return RedirectToPage("/Google/Step1",new { key=key });
            }
            else
            {
                //已绑定，下一步进行双重认证
                return RedirectToPage("/Google/Step3", new { key = key });
            }
        }
 
       
    }
}
