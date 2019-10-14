using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Context
{

    /// <summary>
    /// 微信消息上下文（全局）
    /// 默认过期时间：90分钟
    /// </summary>
    public class GlobalMessageContext<TM, TRequest, TResponse> where TM : class, IMessageContext<TRequest, TResponse>, new() where TRequest : IRequestMessageBase where TResponse : IResponseMessageBase
    {
        private int _maxRecordCount;

        /// <summary>
        /// 所有MessageContext集合，不要直接操作此对象
        /// </summary>
        public Dictionary<string, TM> MessageCollection
        {
            get;
            set;
        }

        /// <summary>
        /// MessageContext队列（LastActiveTime升序排列）,不要直接操作此对象
        /// </summary>
        public MessageQueue<TM, TRequest, TResponse> MessageQueue
        {
            get;
            set;
        }

        /// <summary>
        /// 每一个MessageContext过期时间（分钟）
        /// </summary>
        public double ExpireMinutes
        {
            get;
            set;
        }

        /// <summary>
        /// 最大储存上下文数量（分别针对请求和响应信息）
        /// </summary>
        public int MaxRecordCount
        {
            get;
            set;
        }

        public GlobalMessageContext()
        {
            Restore();
        }

        /// <summary>
        /// 重置所有上下文参数，所有记录将被清空
        /// </summary>
        public void Restore()
        {
            MessageCollection = new Dictionary<string, TM>(StringComparer.OrdinalIgnoreCase);
            MessageQueue = new MessageQueue<TM, TRequest, TResponse>();
            ExpireMinutes = 90.0;
        }

        /// <summary>
        /// 获取MessageContext，如果不存在，返回null
        /// 这个方法的更重要意义在于操作TM队列，及时移除过期信息，并将最新活动的对象移到尾部
        /// </summary>
        /// <param name="userName">用户名（OpenId）</param>
        /// <returns></returns>
        private TM GetMessageContext(string userName)
        {
            while (MessageQueue.Count > 0)
            {
                TM val = MessageQueue[0];
                TimeSpan timeSpan = DateTime.Now - (val.LastActiveTime.HasValue ? val.LastActiveTime.Value : DateTime.Now);
                double num = val.ExpireMinutes.HasValue ? val.ExpireMinutes.Value : ExpireMinutes;
                if (!(timeSpan.TotalMinutes >= num))
                {
                    break;
                }
                MessageQueue.RemoveAt(0);
                MessageCollection.Remove(val.UserName);
                val.OnRemoved();
            }
            if (!MessageCollection.ContainsKey(userName))
            {
                return null;
            }
            return MessageCollection[userName];
        }

        /// <summary>
        /// 获取MessageContext
        /// </summary>
        /// <param name="userName">用户名（OpenId）</param>
        /// <param name="createIfNotExists">true：如果用户不存在，则创建一个实例，并返回这个最新的实例
        /// false：如用户不存在，则返回null</param>
        /// <returns></returns>
        private TM GetMessageContext(string userName, bool createIfNotExists)
        {
            TM messageContext = GetMessageContext(userName);
            if (messageContext == null)
            {
                if (!createIfNotExists)
                {
                    return null;
                }
                Dictionary<string, TM> messageCollection = MessageCollection;
                TM val = new TM();
                val.UserName = userName;
                val.MaxRecordCount = MaxRecordCount;
                messageCollection[userName] = val;
                messageContext = GetMessageContext(userName);
                MessageQueue.Add(messageContext);
            }
            return messageContext;
        }

        /// <summary>
        /// 获取MessageContext，如果不存在，使用requestMessage信息初始化一个，并返回原始实例
        /// </summary>
        /// <returns></returns>
        public TM GetMessageContext(TRequest requestMessage)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                return GetMessageContext(requestMessage.FromUserName, createIfNotExists: true);
            }
        }

        /// <summary>
        /// 获取MessageContext，如果不存在，使用responseMessage信息初始化一个，并返回原始实例
        /// </summary>
        /// <returns></returns>
        public TM GetMessageContext(TResponse responseMessage)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                return GetMessageContext(responseMessage.ToUserName, createIfNotExists: true);
            }
        }

        /// <summary>
        /// 记录请求信息
        /// </summary>
        /// <param name="requestMessage">请求信息</param>
        public void InsertMessage(TRequest requestMessage)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                string userName = requestMessage.FromUserName;
                TM messageContext = GetMessageContext(userName, createIfNotExists: true);
                if (messageContext.RequestMessages.Count > 0)
                {
                    int num = MessageQueue.FindIndex((TM z) => z.UserName == userName);
                    if (num >= 0)
                    {
                        MessageQueue.RemoveAt(num);
                        MessageQueue.Add(messageContext);
                    }
                }
                messageContext.LastActiveTime = messageContext.ThisActiveTime;
                messageContext.ThisActiveTime = DateTime.Now;
                messageContext.RequestMessages.Add(requestMessage);
            }
        }

        /// <summary>
        /// 记录响应信息
        /// </summary>
        /// <param name="responseMessage">响应信息</param>
        public void InsertMessage(TResponse responseMessage)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                GetMessageContext(responseMessage.ToUserName, createIfNotExists: true).ResponseMessages.Add(responseMessage);
            }
        }

        /// <summary>
        /// 获取最新一条请求数据，如果不存在，则返回null
        /// </summary>
        /// <param name="userName">用户名（OpenId）</param>
        /// <returns></returns>
        public TRequest GetLastRequestMessage(string userName)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                return GetMessageContext(userName, createIfNotExists: true).RequestMessages.LastOrDefault();
            }
        }

        /// <summary>
        /// 获取最新一条响应数据，如果不存在，则返回null
        /// </summary>
        /// <param name="userName">用户名（OpenId）</param>
        /// <returns></returns>
        public TResponse GetLastResponseMessage(string userName)
        {
            lock (MessageContextGlobalConfig.Lock)
            {
                return GetMessageContext(userName, createIfNotExists: true).ResponseMessages.LastOrDefault();
            }
        }
    }
}
