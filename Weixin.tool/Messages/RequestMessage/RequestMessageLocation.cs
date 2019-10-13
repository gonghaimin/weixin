using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class RequestMessageLocation : RequestMessageBase, IRequestMessageBase, IMessageBase
    {
        public override RequestMsgType MsgType => RequestMsgType.location;

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Location_X
        {
            get;
            set;
        }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y
        {
            get;
            set;
        }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale
        {
            get;
            set;
        }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get;
            set;
        }

        public RequestMessageLocation()
        {
        }
    }
}
