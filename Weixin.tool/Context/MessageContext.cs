using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Context
{

    /// <summary>
    /// 微信消息上下文（单个用户）
    /// </summary>
    public class MessageContext<TRequest, TResponse> : IMessageContext<TRequest, TResponse>  where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
    {
        private int _maxRecordCount;

        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 最后一次活动时间（用户主动发送Resquest请求的时间）
        /// </summary>
        public DateTimeOffset? LastActiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 本次活动时间（当前消息收到的时间）
        /// </summary>
        public DateTimeOffset? ThisActiveTime
        {
            get;
            set;
        }

        public MessageContainer<TRequest> RequestMessages
        {
            get;
            set;
        }

        public MessageContainer<TResponse> ResponseMessages
        {
            get;
            set;
        }

        public int MaxRecordCount
        {
            get
            {
                return _maxRecordCount;
            }
            set
            {
                RequestMessages.MaxRecordCount = value;
                ResponseMessages.MaxRecordCount = value;
                _maxRecordCount = value;
            }
        }

        public object StorageData
        {
            get;
            set;
        }

        public double? ExpireMinutes
        {
            get;
            set;
        }



        /// <summary>
        /// 当MessageContext被删除时触发的事件
        /// </summary>
        public virtual event EventHandler<WeixinContextRemovedEventArgs<TRequest, TResponse>> MessageContextRemoved;

        /// <summary>
        /// 执行上下文被移除的事件
        /// 注意：此事件不是实时触发的，而是等过期后任意一个人发过来的下一条消息执行之前触发。
        /// </summary>
        /// <param name="e"></param>
        private void OnMessageContextRemoved(WeixinContextRemovedEventArgs<TRequest, TResponse> e)
        {
            this.MessageContextRemoved?.Invoke(this, e);
        }

        /// <summary>
        ///
        /// </summary>
        public MessageContext()
        {
            RequestMessages = new MessageContainer<TRequest>(MaxRecordCount);
            ResponseMessages = new MessageContainer<TResponse>(MaxRecordCount);
            LastActiveTime = DateTime.Now;
        }

        /// <summary>
        /// 此上下文被清除的时候触发
        /// </summary>
        public virtual void OnRemoved()
        {
            WeixinContextRemovedEventArgs<TRequest, TResponse> e = new WeixinContextRemovedEventArgs<TRequest, TResponse>(this);
            OnMessageContextRemoved(e);
        }
    }

}
