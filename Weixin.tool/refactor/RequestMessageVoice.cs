using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Utility;

namespace Weixin.Tool.refactor
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class RequestMessageVoice : RequestMessageBase, IRequestMessageBase
    {
        /// <summary>
        /// 语音格式：amr
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音消息媒体id，可以调用获取临时素材接口拉取该媒体
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }
        public override RequestMsgType MsgType => RequestMsgType.voice;

    }
}
