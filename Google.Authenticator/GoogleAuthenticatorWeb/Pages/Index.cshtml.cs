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


namespace GoogleAuthenticatorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDistributedCache _cache;
        public IndexModel(ILogger<IndexModel> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public void OnGet()
        {

        }
    }
}
