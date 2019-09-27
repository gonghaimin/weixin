using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleAuthenticatorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace GoogleAuthenticatorWeb.Pages.Google
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public LoginUser User { get; set; }
        public IActionResult OnGet()
        {
            var str = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(str))
            {
                //未登录
                return RedirectToPage("/Google/Login");
            }
            else
            {
                User = JsonConvert.DeserializeObject<LoginUser>(str);
            }
            return Page();
        }
        public  IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();

            return RedirectToPage("/Google/Login");
        }
    }
}