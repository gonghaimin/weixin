using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Weixin.Tool.Utility
{
    //
    // 摘要:
    //     /// 签名验证类 ///
    public class CheckSignature
    {

        public static bool Check(string signature, string timestamp, string nonce, string token = null)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }

        public static string GetSignature(string timestamp, string nonce, string token = null)
        {
            token = (token ?? "weixin");
            string[] value = (from z in new string[3]
            {
                token,
                timestamp,
                nonce
            } orderby z select z).ToArray();
            string s = string.Join("", value);
            byte[] array = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            foreach (byte b in array2)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}
