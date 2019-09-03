using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Tool.Handlers
{
    /// <summary>
    /// 处理接口
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 1、处理请求。
        /// 2、一定要明确：回复的消息类型不一定要与请求的消息类型一样。
        /// 3、但必须是可用于回复的，当前支持的有：文本、图文、音乐等。
        /// </summary>
        /// <returns></returns>
        string HandleRequest();
    }
}
