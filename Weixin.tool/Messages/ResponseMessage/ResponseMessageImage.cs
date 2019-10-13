using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ResponseMessageImage : ResponseMessageBase, IResponseMessageBase, IMessageBase
    {
        public override ResponseMsgType MsgType => ResponseMsgType.Image;

        public Image Image
        {
            get;
            set;
        }

        public ResponseMessageImage()
        {
            Image = new Image();
        }
    }
    public class Image
    {
        public string MediaId
        {
            get;
            set;
        }
    }
}
