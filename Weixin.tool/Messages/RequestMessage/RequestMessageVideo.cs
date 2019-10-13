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
    /// 视频消息
    /// </summary>
    public class RequestMessageVideo : RequestMessageBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.video;

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId
        {
            get;
            set;
        }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId
        {
            get;
            set;
        }

        public RequestMessageVideo()
        {
        }
    }
}
