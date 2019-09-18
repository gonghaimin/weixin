using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class RedpackRequest
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public String SendName { get; set; }

        public String OpenId { get; set; }
        public decimal Amount { get; set; }
        public String Wishing { get; set; }
        public String ActivityName { get; set; }
        public String Remark { get; set; }
        public String ClientIP { get; set; }
    }
}
