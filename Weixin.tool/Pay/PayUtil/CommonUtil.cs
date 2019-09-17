using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Weixin.Tool.Pay.PayUtil
{
    public class CommonUtil
    {

        public static String CreateNoncestr(int length = 16)
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }



        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        internal static SortedDictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            SortedDictionary<string, string> dicArray = new SortedDictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }



        private static bool IsNumeric(String str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static string ArrayToXml(Dictionary<string, string> arr)
        {
            String xml = "<xml>";

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (IsNumeric(val))
                {
                    xml += "<" + key + ">" + val + "</" + key + ">";

                }
                else
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }

        internal static string ArrayToXml(SortedDictionary<string, string> arr)
        {
            String xml = "<xml>";

            String[] cDATAParams = new string[] { "attach", "body", "sign", "result_code", "err_code_des" };

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (cDATAParams.Contains(key, StringComparer.OrdinalIgnoreCase))
                {
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
                }
                else
                    xml += "<" + key + ">" + val + "</" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }


        /// <summary>
        /// 获取服务器通知数据方式，进行参数获取
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static SortedDictionary<String, String> GetRequest(HttpContext httpContext, out SortedDictionary<String, String> xmlParams)
        {
            SortedDictionary<String, String> sArray = new SortedDictionary<String, String>();
            xmlParams = new SortedDictionary<string, string>();

            dynamic collection = null;
            
            if (httpContext.Request.Method == "POST")
            {
                collection = httpContext.Request.Form;
            }

            if (collection.Count() == 0)
            {
                collection = httpContext.Request.Query;
            }

            foreach (string k in collection)
            {
                if (!String.IsNullOrEmpty(k))
                {
                    string v = (string)collection[k];
                    sArray.Add(k, v);
                }
            }

            if (httpContext.Request.Body.Length > 0)
            {
                try
                {
                    var inputStream = httpContext.Request.Body;
                    inputStream.Seek(0, System.IO.SeekOrigin.Begin);

                    var inputString = new System.IO.StreamReader(inputStream).ReadToEnd();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(inputString);
                    XmlNode root = xmlDoc.SelectSingleNode("xml");
                    XmlNodeList xnl = root.ChildNodes;

                    foreach (XmlNode xnf in xnl)
                    {
                        xmlParams.Add(xnf.Name, xnf.InnerText);
                    }
                }
                catch
                {

                }
            }

            return sArray;
        }


        private static SortedDictionary<String, String> GetRequestParams(HttpContext httpContext)
        {
            SortedDictionary<String, String> requestParams = new SortedDictionary<String, String>();
            String url = httpContext.Request.QueryString.ToString();
            int l = url.IndexOf('?');
            if (l >= 0)
            {
                url = url.Substring(l + 1);
                String[] urls = url.Split('&');
                for (int i = 0; i < urls.Length; i++)
                {
                    requestParams.Add(urls[i].Split('=')[0], urls[i].Split('=')[1]);
                }
            }

            return requestParams;
        }
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(IDictionary<string, string> dictionary, bool urlencode = false, string input_charset = "UTF-8", bool? ignoreNull = null)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dictionary)
            {
                if (temp.Key.Equals("sign", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (ignoreNull != null && ignoreNull.Value && temp.Value == null)
                {
                    continue;
                }

                if (urlencode)
                {
                    prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, Encoding.GetEncoding(input_charset)) + "&");
                }
                else
                {
                    prestr.Append(temp.Key + "=" + temp.Value + "&");
                }
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
    }
}
