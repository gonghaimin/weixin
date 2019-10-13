using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Messages.Base
{
    public interface IRequestMessageEventBase:IRequestMessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        Event Event { get;  }
        /// <summary>
        /// 事件key，与自定义菜单接口中的key值对应
        /// </summary>
        string EventKey { get; set; }
    }
}
