using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Messages;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class LocationHandler : IHandler
    {
        /// <summary>
        /// 请求的XML
        /// </summary>
        private string RequestXml { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestXml">请求的xml</param>
        public LocationHandler(string requestXml)
        {
            this.RequestXml = requestXml;
        }
        public string HandleRequest()
        {
            string response = string.Empty;
            LocationMessage lm = LocationMessage.LoadFromXml(RequestXml);
            TextMessage tm = new TextMessage();
            lm.ConvertToIReplyMessage(tm);
            tm.Content = Newtonsoft.Json.JsonConvert.SerializeObject(lm);
            response = tm.GetResponse();
            return response;
        }
    }
}
