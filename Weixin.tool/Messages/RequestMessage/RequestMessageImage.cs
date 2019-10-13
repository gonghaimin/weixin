using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.image;

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId
        {
            get;
            set;
        }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl
        {
            get;
            set;
        }

        public RequestMessageImage()
        {
        }
    }
}
