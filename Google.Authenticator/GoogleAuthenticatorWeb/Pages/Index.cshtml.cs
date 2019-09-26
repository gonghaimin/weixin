using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GoogleAuthenticatorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static GoogleAuthenticator.GoogleAuthenticator;

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
        [BindProperty]
        public new User User { get; set; }
        public string QrCodeImageUrl { get; set; }
        public void OnGet()
        {
    
        }
        public void OnPostAsync()
        {
            var key = Md5EncryptStr32(User.Name+User.PassWord);
            var cachestr = _cache.GetString(key);
            if (string.IsNullOrEmpty(cachestr))
            {
                TwoFactorAuthenticator ga = new TwoFactorAuthenticator();
                var setupCode = ga.GenerateSetupCode(User.Name);
                QrCodeImageUrl = setupCode.QrCodeImageUrl;
                User.SecretKey = setupCode.SecretKey;
                _cache.SetString(key, JsonConvert.SerializeObject(User));
            }
            else
            {
                User = JsonConvert.DeserializeObject<User>(cachestr);

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
