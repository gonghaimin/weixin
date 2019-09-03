using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    public class ShortVideoMessage: VideoMessage
    {
        public ShortVideoMessage()
        {
            this.MsgType = MsgTypeEnum.shortvideo.ToString();
        }
    }
}
