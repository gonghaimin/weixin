using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Weixin.Tool.Utility
{
    /// <summary>
    /// 公共功能
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 加密签名
        /// </summary>
        public const string Signature = "signature";
        /// <summary>
        /// 时间戳
        /// </summary>
        public const string Timestamp = "timestamp";
        /// <summary>
        /// 随机数
        /// </summary>
        public const string Nonce = "nonce";
        /// <summary>
        /// 随机字符串
        /// </summary>
        public const string Echostr = "echostr";

        /// <summary>
		/// 发送人（OpenID）
        /// </summary>
        public const string FromUserName = "FromUserName";
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public const string ToUserName = "ToUserName";
        /// <summary>
        /// 消息内容
        /// </summary>
        public const string Content = "Content";
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public const string CreateTime = "CreateTime";
        /// <summary>
        /// 消息类型
        /// </summary>
        public const string MsgType = "MsgType";
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public const string MsgId = "MsgId";

        public const string MediaId = "MediaId";

        public const string PicUrl = "PicUrl";

        public const string Format = "Format";

        public const string ThumbMediaId = "ThumbMediaId";

        public const string Location_X = "Location_X";

        public const string Location_Y = "Location_Y";

        public const string Scale = "Scale";

        public const string Label = "Label";
        /// <summary>
        /// 得到当前时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetNowTime()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long time = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
            return time.ToString();
        }
        /// <summary>
        /// 读取请求对象的内容
        /// 只能读一次
        /// </summary>
        /// <param name="request">HttpRequest对象</param>
        /// <returns>string</returns>
        public static string ReadRequest(HttpRequest request)
        {
            string reqStr = string.Empty;
            using (Stream s = request.Body)
            {
                using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
                {
                    reqStr = reader.ReadToEnd();
                }
            }
            return reqStr;
        }
    }
}
