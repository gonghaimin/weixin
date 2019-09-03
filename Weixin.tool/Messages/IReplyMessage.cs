using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 被动回复消息
    /// </summary>
    public abstract class IReplyMessage: BaseMessage
    {
        /// <summary>
        /// 基本必填项校验
        /// </summary>
        /// <returns></returns>
        private bool VerifyBaseParmeter(out string msg)
        {
            msg = string.Empty;
            if (String.IsNullOrEmpty(this.ToUserName))
            {
                msg = "ToUserName";
                return false;
            }
            if (string.IsNullOrEmpty(this.FromUserName))
            {
                msg = "FromUserName";
                return false;
            }
            if (string.IsNullOrEmpty(this.CreateTime))
            {
                msg = "CreateTime";
                return false;
            }
            if (string.IsNullOrEmpty(this.MsgType))
            {
                msg = "MsgType";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 必填项校验
        /// </summary>
        /// <returns></returns>
        protected virtual bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            return true;
        }
        /// <summary>
        /// 生成回复消息内容
        /// </summary>
        /// <returns>string</returns>
        protected abstract string GenerateContent();
        /// <summary>
        /// 获取响应用户消息
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            string msg;
            if(VerifyBaseParmeter(out msg))
            {
                if (VerifyParameter(out msg))
                {
                    return GenerateContent();
                }
            }
            throw new Exception("缺少必填参数:" + msg);
        }
    }
}
