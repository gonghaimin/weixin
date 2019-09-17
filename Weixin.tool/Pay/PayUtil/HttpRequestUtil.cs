using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Weixin.Tool.Pay.PayUtil
{
    public class HttpRequestUtil
    {
        private readonly static int IntervalSeed = 2;

        public static string Send(string verb, string url, string postData, int timeout = 10, X509Certificate cer = null)
        {
            var result = Policy.Handle<Exception>().WaitAndRetry(2, t => TimeSpan.FromSeconds(IntervalSeed * t))
                 .Execute(() => SendCore(verb, url, postData, timeout, cer));
            return result;
        }
        private static string SendCore(string verb, string url, string postData, int timeout = 10, X509Certificate cer = null)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = timeout * 1000;

            req.Method = verb;
            String responseBody = "";

            try
            {
                if (verb == "POST")
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);

                    req.ContentType = "text/xml;encoding='utf-8'";
                    req.ContentLength = data.Length;
                    if (cer != null)
                    {
                        req.ClientCertificates.Add(cer);
                    }

                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }

                using (var response = (HttpWebResponse)req.GetResponse())

                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {

                                responseBody = reader.ReadToEnd();

                            }
                        }
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                responseBody = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (Stream stream = response.GetResponseStream())
                        {

                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                responseBody = reader.ReadToEnd();
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                //日志
            }

            return responseBody;
        }

        public static string SendJson<TRequest>(string url, TRequest request, string requestMethod = "Get", Dictionary<string, string> headers = null)
        {
            var result = Policy.Handle<Exception>().WaitAndRetry(2, t => TimeSpan.FromSeconds(IntervalSeed * t))
                 .Execute(() => SendJsonCore(url, request, requestMethod, headers));
            return result;
        }
        private static string SendJsonCore<TRequest>(string url, TRequest request, string requestMethod = "Get", Dictionary<string, string> headers = null)
        {
            string result = string.Empty;
            Stream stream = null;
            StreamReader streamReader = null;
            try
            {
                Uri uri = new Uri(url);
                var httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.Method = requestMethod;
                httpRequest.Timeout = 30000;
                httpRequest.ContentType = "application/json"; //"application/x-www-form-urlencoded";
                                                              //System.Net.ServicePointManager.Expect100Continue = false;

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                }

                if (requestMethod.Equals("Post", StringComparison.OrdinalIgnoreCase))
                {
                    httpRequest.ServicePoint.Expect100Continue = false;
                }
                if (request != null)
                {
                    var data = JsonConvert.SerializeObject(request);
                    var bytes = Encoding.UTF8.GetBytes(data);
                    httpRequest.ContentLength = bytes.Length;
                    stream = httpRequest.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                var response = (HttpWebResponse)httpRequest.GetResponse();
                stream = response.GetResponseStream();
                if (stream != null)
                {
                    streamReader = new StreamReader(stream, Encoding.UTF8);
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamReader.Dispose();
                    stream.Close();
                    stream.Dispose();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (streamReader != null)
                    streamReader.Close();
            }
            return result;
        }
    }
}
