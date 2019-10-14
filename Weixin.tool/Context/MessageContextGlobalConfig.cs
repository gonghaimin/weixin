using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Context
{
    /// <summary>
    /// 消息上下文全局设置
    /// </summary>
    public static class MessageContextGlobalConfig
    {
        /// <summary>
        /// 上下文操作使用的同步锁
        /// </summary>
        public static object Lock = new object();

        /// <summary>
        /// 去重专用锁
        /// </summary>
        public static object OmitRepeatLock = new object();

        /// <summary>
        /// 是否开启上下文记录
        /// </summary>
        public static bool UseMessageContext = true;

        /// <summary>
        /// 是否开启上下文记录
        /// </summary>
        [Obsolete("请使用 UseMessageContext")]
        public static bool UseWeixinContext
        {
            get
            {
                return UseMessageContext;
            }
            set
            {
                UseMessageContext = value;
            }
        }
    }
}
