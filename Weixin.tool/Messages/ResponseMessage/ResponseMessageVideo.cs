using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ResponseMessageVideo : ResponseMessageBase, IResponseMessageBase, IMessageBase
    {
        public override ResponseMsgType MsgType => ResponseMsgType.Video;

        public Video Video
        {
            get;
            set;
        }

        public ResponseMessageVideo()
        {
            Video = new Video();
        }
    }

    public class Video
    {
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
