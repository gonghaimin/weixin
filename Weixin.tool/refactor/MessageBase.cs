using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.refactor
{
    //
    // 摘要:
    //     /// 所有Request和Response消息的接口 ///
    public class MessageBase
    {
        //
        // 摘要:
        //     /// 接收人（在 Request 中为公众号的微信号，在 Response 中为 OpenId） ///
        public string ToUserName
        {
            get;
            set;
        }

        //
        // 摘要:
        //     /// 发送人（在 Request 中为OpenId，在 Response 中为公众号的微信号） ///
        public string FromUserName
        {
            get;
            set;
        }

        //
        // 摘要:
        //     /// 消息创建时间 ///
        public long CreateTime
        {
            get;
            set;
        }
    }
}
