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
using static GoogleAuthenticator.GoogleAuthenticator;

namespace GoogleAuthenticatorWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IDistributedCache _cache;
        public LoginModel(ILogger<LoginModel> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        [BindProperty]
        public new User User { get; set; }
        [BindProperty]
        public bool IsLogin { get { return User != null; } } 
        public string QrCodeImageUrl { get; set; }
        public string Token { get; set; }
        public string SecretKey { get; set; }
        public string Msg { get; set; }
        public void OnGet()
        {
            HttpContext.Session.SetString("test", "ghm");//存储在IDistributedCache
            var str = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(str))
            {
                User = JsonConvert.DeserializeObject<User>(str);
            }

        }
        public void OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            var key = Md5EncryptStr32(User.Name + User.PassWord);
            var cachestr = _cache.GetString(key);
            if (!string.IsNullOrEmpty(cachestr))
            {
                User = JsonConvert.DeserializeObject<User>(cachestr);
            }
            else
            {
                cachestr = JsonConvert.SerializeObject(User);
                _cache.SetString(key, cachestr);
                HttpContext.Session.SetString("user", cachestr);
            }
            if (string.IsNullOrEmpty(User.SecretKey))
            {
                TwoFactorAuthenticator ga = new TwoFactorAuthenticator();
                var setupCode = ga.GenerateSetupCode(User.Name);
                QrCodeImageUrl = setupCode.QrCodeImageUrl;
                SecretKey = setupCode.SecretKey;
            }
        }
        public void OnPostStepAsync()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                TwoFactorAuthenticator ga = new TwoFactorAuthenticator();
                if(ga.ValidateTwoFactorPIN(User.SecretKey, Token))
                {
                    Msg = "登录成功";
                }
                else
                {
                    Msg = "登录失败";
                }
            }
        }
        //对密码进行MD5加密
        static string Md5EncryptStr32(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }
    }
}
