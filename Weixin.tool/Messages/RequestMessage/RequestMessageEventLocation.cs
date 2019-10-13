using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.RequestMessage
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    public class RequestMessageEventLocation : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageBase, IMessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event => Event.LOCATION;

        /// <summary>
        /// 地理位置维度，事件类型为LOCATION的时存在
        /// </summary>
        public double Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// 地理位置经度，事件类型为LOCATION的时存在
        /// </summary>
        public double Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// 地理位置精度，事件类型为LOCATION的时存在
        /// </summary>
        public double Precision
        {
            get;
            set;
        }
    }
}
