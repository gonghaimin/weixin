using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.WxResults;

namespace Weixin.Tool.Utility
{
    //https://www.cnblogs.com/szw/p/weixin-sdk-request-proxy.html
    public static class ApiHandler
    {
        /// <summary>
        /// 使用AccessToken进行操作时，如果遇到AccessToken错误的情况，重新获取AccessToken一次，并重试。 /// 使用此方法之前必须在startup注册  WeiXinContext.RegisterWX
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static T TryCommonApi<T>(Func<string, T> fun, bool retryIfFaild = true) where T : WxJsonResult, new()
        {
            var access_token = WeiXinContext.AccessToken;
            var result = fun(access_token);
            if (result.ReturnCode == ReturnCode.请求成功)
            {
                return result;
            }
            if (result.ReturnCode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
            {
                if (retryIfFaild)
                {
                    WeiXinContext.ClearAccessToken();
                    return TryCommonApi<T>(fun, false);
                }
            }
            throw new Exception(result.ToString());
        }
        public static Dictionary<string,object> TryCommonApi(Func<string, Dictionary<string, object>> fun, bool retryIfFaild = true) 
        {
            var access_token = WeiXinContext.AccessToken;
            var result = fun(access_token);
            if (!result.ContainsKey("errcode"))
            {
                return result;
            }
            if (result["errcode"].ToString() ==(int) ReturnCode.请求成功+"")
            {
                return result;
            }
            if (result["errcode"].ToString() ==(int) ReturnCode.获取access_token时AppSecret错误或者access_token无效+"")
            {
                if (retryIfFaild)
                {
                    WeiXinContext.ClearAccessToken();
                    return TryCommonApi(fun, false);
                }
            }
            throw new Exception(result.ToString());
        }
        public static string Get(string url,int timeOut=1000, Action<string, string> afterReturnText=null, Dictionary<string, string> header = null)
        {
            HttpClient client = WeiXinContext.Client;
          
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Timeout", timeOut.ToString());
            client.DefaultRequestHeaders.Add("KeepAlive", "true");
            
            if (header != null)
            {
                foreach (var item in header)
                {
                    if(!client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
            }
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var text = response.Content.ReadAsStringAsync().Result;
            afterReturnText?.Invoke(url, text);
            return text;
        }

        public static T GetJson<T>(string url, int timeOut = 1000, Action<string, string> afterReturnText = null)
        {
            var text = Get(url, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static Dictionary<string, object> GetDict(string url, int timeOut = 1000, Action<string, string> afterReturnText = null)
        {
            var text = Get(url, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        }
        public static string Post(string url, HttpContent content,  int timeOut = 1000, Action<string, string, string> afterReturnText = null, Dictionary<string, string> header = null)
        {
            HttpClient client = WeiXinContext.Client;
            client.DefaultRequestHeaders.Add("Timeout", timeOut.ToString());
            client.DefaultRequestHeaders.Add("KeepAlive", "true");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (header != null)
            {
                foreach (var item in header)
                {
                    if (!client.DefaultRequestHeaders.Contains(item.Key))
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
            }
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            var response = client.SendAsync(request).Result;
         
            response.EnsureSuccessStatusCode();
            var text = response.Content.ReadAsStringAsync().Result;
            var param = content.ReadAsStringAsync().Result;
            afterReturnText?.Invoke(url, param, text);
            return text;
        }
        public static T PostGetJson<T>(string url, HttpContent content, int timeOut = 1000, Action<string, string, string> afterReturnText = null)
        {
            var text = Post(url, content, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static Dictionary<string, object> PostDict(string url, HttpContent content, int timeOut = 1000, Action<string, string, string> afterReturnText = null)
        {
            var text = Post(url, content, timeOut, afterReturnText);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
        }
        /// <summary>
        /// 从url下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public static void Download(string url, Stream stream)
        {
            Task<byte[]> byteArrayAsync = WeiXinContext.Client.GetByteArrayAsync(url);
            byteArrayAsync.Wait();
            byte[] result = byteArrayAsync.Result;
            stream.Write(result, 0, result.Length);
        }

        //
        // 摘要:
        //     /// 从Url下载，并保存到指定目录 ///
        //
        // 参数:
        //   url:
        //     需要下载文件的Url
        //
        //   filePathName:
        //     保存文件的路径，如果下载文件包含文件名，按照文件名储存，否则将分配Ticks随机文件名
        //
        //   timeOut:
        //     超时时间
        public static string Download(string url, string filePathName=null,string extension=".jpg" ,int timeOut = 999)
        {
            string text = null;
            if (!string.IsNullOrEmpty(filePathName))
            {
                text = Path.GetDirectoryName(filePathName) ?? "/";
            }
            else
            {
                text = Directory.GetCurrentDirectory();
            }
           
            Directory.CreateDirectory(text);
            using (HttpResponseMessage httpResponseMessage = WeiXinContext.Client.GetAsync(url).Result)
            {
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    string text2 = null;
                    if (httpResponseMessage.Content.Headers.ContentDisposition != null && httpResponseMessage.Content.Headers.ContentDisposition.FileName != null && httpResponseMessage.Content.Headers.ContentDisposition.FileName != "\"\"")
                    {
                        text2 = Path.Combine(text, httpResponseMessage.Content.Headers.ContentDisposition.FileName.Trim(new char[1]
                        {
                            '"'
                        }));
                    }
                    string text3 = text2 ?? Path.Combine(text, GetRandomFileName(extension));
                    using (FileStream fileStream = File.Open(text3, FileMode.Create))
                    {
                        using (Stream stream = httpResponseMessage.Content.ReadAsStreamAsync().Result)
                        {
                            stream.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                    }
                    return text3;
                }
                return null;
            }
        }
        //
        // 摘要:
        //     /// 获取随机文件名 ///
        private static string GetRandomFileName(string extension=null)
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6)+ extension;
        }
        /// <summary>
        /// 上传文件的同时上传其他参数
        /// </summary>
        /// <param name="fileDictionary"></param>
        /// <returns></returns>
        public static MultipartFormDataContent CreateMultipartFormDataContent(Dictionary<string, string> fileDictionary)
        {
            if (fileDictionary != null && fileDictionary.Count > 0)
            {
                string text = "----" + DateTime.Now.Ticks.ToString("x");
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent(text);
                foreach (KeyValuePair<string, string> item in fileDictionary)
                {
                    try
                    {
                        string value = item.Value;
                        string fileName = null;
                        MemoryStream memoryStream = new MemoryStream();
                        using (FileStream fileStream = FileUtility.GetFileStream(value))
                        {
                            if (fileStream != null)
                            {
                                fileStream.CopyTo(memoryStream);
                                fileName = Path.GetFileName(value);
                                fileStream.Dispose();
                            }
                            else
                            {
                                multipartFormDataContent.Add(new StringContent(item.Value), "\"" + item.Key + "\"");
                            }
                        }
                        if (memoryStream.Length > 0)
                        {
                            memoryStream.Seek(0L, SeekOrigin.Begin);
                            multipartFormDataContent.Add(CreateFileContent(memoryStream, item.Key, fileName), item.Key, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                multipartFormDataContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={text}");
                return multipartFormDataContent;
            }
            return null;
        }
        /// <summary>
        /// 创建文件 httpcontent
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="formName"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static StreamContent CreateFileContent(Stream stream, string formName, string fileName, string contentType = "application/octet-stream")
        {
            StreamContent streamContent = new StreamContent(stream);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{formName}\"",
                FileName = $"\"{fileName}\"",
                Size = stream.Length
            };
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return streamContent;
        }
    }
}
