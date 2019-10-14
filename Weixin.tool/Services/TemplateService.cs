using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Services
{
    public class TemplateService:IService
    {
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> get_all_private_template()
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var result = ApiHandler.GetDict(string.Format(ApiConfig.get_all_private_template, accessToken));

                return result;
            });
        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> templateSend(string touser= "o6QJo6Cw-P9xvcg2ok38k6PwKifQ")
        {
            var dict = new Dictionary<string, object>();
            dict["touser"] = touser;
            dict["template_id"] = "Xb0CvU59qECsmcOyiG6HYcc540g1MEDyReHCeSPB-n4";
            dict["url"] = "https://www.runoob.com/python3/python3-basic-syntax.html";
            //跳转小程序设置
            //dict["miniprogram"] = new Dictionary<string, string> {
            //    {"appid","xiaochengxuappid12345" },
            //    {"pagepath","index?foo=bar" }
            //};
            dict["data"] = new Dictionary<string, object>() {
                { "first" ,new {value="恭喜你购买成功！",color="#173177"} },
                { "keyword1" ,new {value="巧克力",color="#173177"} },
                { "keyword2" ,new {value="39.8元",color="#173177"} },
                { "keyword3" ,new {value="2014年9月22日",color="#173177"} },
                { "remark" ,new {value="欢迎再次购买！",color="#173177"} }
            };
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dict));
                var result = ApiHandler.PostDict(string.Format(ApiConfig.templateSend, accessToken), content);

                return result;
            });
        }
    }
}
