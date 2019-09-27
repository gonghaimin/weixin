using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GoogleAuthenticatorWeb.Pages.Google
{
    public class Step1Model : PageModel
    {
        private readonly ILogger<Step1Model> _logger;
        private readonly IDistributedCache _db;
        public Step1Model(ILogger<Step1Model> logger, IDistributedCache cache)
        {
            _logger = logger;
            _db = cache;
        }
        [BindProperty]
        public string unid { get; set; }
        public IActionResult OnGet(string key)
        {
            unid = key;
            return Page();
        }
    }
}