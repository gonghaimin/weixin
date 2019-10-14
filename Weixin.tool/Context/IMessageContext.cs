using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Context
{
    /// <summary>
    /// 微信消息上下文（单个用户）接口
    /// </summary>
    /// <typeparam name="TRequest">请求消息类型</typeparam>
    /// <typeparam name="TResponse">响应消息类型</typeparam>
    public interface IMessageContext<TRequest, TResponse> where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
    {
        /// <summary>
        /// 用户名（OpenID）
        /// </summary>
        string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 最后一次活动时间（用户主动发送Resquest请求的时间）
        /// </summary>
        DateTimeOffset? LastActiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 本次活动时间（当前消息收到的时间）
        /// </summary>
        DateTimeOffset? ThisActiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 接收消息记录
        /// </summary>
        MessageContainer<TRequest> RequestMessages
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息记录
        /// </summary>
        MessageContainer<TResponse> ResponseMessages
        {
            get;
            set;
        }

        /// <summary>
        /// 最大储存容量（分别针对RequestMessages和ResponseMessages）
        /// </summary>
        int MaxRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// 临时储存数据，如用户状态等，出于保持.net 3.5版本，这里暂不使用dynamic
        /// </summary>
        object StorageData
        {
            get;
            set;
        }

        /// <summary>
        /// 用于覆盖WeixinContext所设置的默认过期时间
        /// </summary>
        double? ExpireMinutes
        {
            get;
            set;
        }



        event EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> MessageContextRemoved;

        void OnRemoved();
    }

}
