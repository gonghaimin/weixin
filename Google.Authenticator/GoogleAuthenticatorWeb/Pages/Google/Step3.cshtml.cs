using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleAuthenticator;
using GoogleAuthenticatorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoogleAuthenticatorWeb.Pages.Google
{
    public class Step3Model : PageModel
    {
        private readonly ILogger<Step3Model> _logger;
        private readonly IDistributedCache _db;
        public Step3Model(ILogger<Step3Model> logger, IDistributedCache cache)
        {
            _logger = logger;
            _db = cache;
        }
        [BindProperty]
        public static string unid { get; set; }
        [BindProperty]
        public string Code { get; set; }
        public void OnGet(string key)
        {
            unid = key;
        }
        public async Task<IActionResult> OnPostChcekAsync()
        {
            if (!string.IsNullOrEmpty(Code))
            {
                var str = await _db.GetStringAsync(unid);
                var user = JsonConvert.DeserializeObject<LoginUser>(str);
                TwoFactorAuthenticator ta = new TwoFactorAuthenticator();
                var result = ta.ValidateTwoFactorPIN(user.SecretKey, Code);
                if (result)
                {
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                    return RedirectToPage("/Google/Index");
                }
            }
            return Page();
        }
    }
}