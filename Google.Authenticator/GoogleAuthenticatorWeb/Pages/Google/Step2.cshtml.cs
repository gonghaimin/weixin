using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleAuthenticator;
using GoogleAuthenticatorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoogleAuthenticatorWeb.Pages.Google
{
    public class Step2Model : PageModel
    {
        private readonly ILogger<Step2Model> _logger;
        private readonly IDistributedCache _db;
        public Step2Model(ILogger<Step2Model> logger, IDistributedCache cache)
        {
            _logger = logger;
            _db = cache;
        }
        [BindProperty]
        public string unid { get; set; }

        [BindProperty]
        public SetupCode CodeData { get; set; }
        public void OnGet(string key)
        {
            unid = key;
            var str=_db.GetString(key);
            var user = JsonConvert.DeserializeObject<LoginUser>(str);
            TwoFactorAuthenticator ta = new TwoFactorAuthenticator();
            if (string.IsNullOrEmpty(user.SecretKey))
            {
                CodeData = ta.GenerateSetupCode(user.Name);
                user.SecretKey = CodeData.SecretKey;
                _db.SetString(key, JsonConvert.SerializeObject(user));
            }
            else
            {
                CodeData = ta.GenerateSetupCode(user.Name,user.SecretKey,true);
            }
            
        }
    }
}