using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 回复消息模板接口
    /// </summary>
    public abstract class IReplyMessage: Message
    {
        /// <summary>
        /// 必填项校验
        /// </summary>
        /// <returns></returns>
        protected virtual bool VerifyParameter()
        {
            return true;
        }
        /// <summary>
        /// 生成回复消息内容
        /// </summary>
        /// <returns>string</returns>
        protected abstract string GenerateContent();

        protected string GetResponse()
        {
            VerifyParameter();
            return GenerateContent();
        }
    }
}
