using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Services
{
    /// <summary>
    /// 客服
    /// </summary>
   public  class CustomService:IService
    {
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> CustomSendText(CustomSend send)
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(send));
                var result = ApiHandler.PostGetJson<Dictionary<string, object>>(string.Format(ApiConfig.customSend, accessToken), content);

                return result;
            });
        }
    }
}
