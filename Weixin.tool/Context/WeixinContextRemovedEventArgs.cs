using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Context
{
    /// <summary>
    /// 对话上下文被删除时触发事件的事件数据
    /// </summary>
    public class WeixinContextRemovedEventArgs<TRequest, TResponse> : EventArgs where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
    {
        /// <summary>
        /// 该用户的OpenId
        /// </summary>
        public string OpenId => MessageContext.UserName;

        /// <summary>
        /// 最后一次响应时间
        /// </summary>
        public DateTimeOffset LastActiveTime
        {
            get
            {
                if (!MessageContext.LastActiveTime.HasValue)
                {
                    return DateTimeOffset.MinValue;
                }
                return MessageContext.LastActiveTime.Value;
            }
        }

        /// <summary>
        /// 上下文对象
        /// </summary>
        public MessageContext<TRequest, TResponse> MessageContext
        {
            get;
            set;
        }

        public WeixinContextRemovedEventArgs(MessageContext<TRequest, TResponse> messageContext)
        {
            MessageContext = messageContext;
        }
    }
}
