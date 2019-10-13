using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    public class ResponseMessageVoice : ResponseMessageBase, IResponseMessageBase, IMessageBase
    {
        public override ResponseMsgType MsgType => ResponseMsgType.Voice;

        public Voice Voice
        {
            get;
            set;
        }

        public ResponseMessageVoice()
        {
            Voice = new Voice();
        }
    }

    public class Voice
    {
        public string MediaId { get; set; }
    }
}
