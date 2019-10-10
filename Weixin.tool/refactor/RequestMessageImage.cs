using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.refactor
{
    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.image;
              /// <summary>
                /// 图片链接（由系统生成）
                /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public string MediaId { get; set; }
    }
}
