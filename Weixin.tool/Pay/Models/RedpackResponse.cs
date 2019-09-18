using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class RedpackResponse
    {
        public String RetCode { get; set; }

        /// <summary>
        /// 返回信息，如非空，为错误原因
        /// </summary>
        public String RetMsg { get; set; }

        public string SendListId { get; set; }
    }
}
