using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class RefundRequest
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public String OutTradeNO { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public String TransactionID { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        public String RefundNO { get; set; }

        /// <summary>
        /// 支付的总金额,单位元
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 退款的总金额,单位元
        /// </summary>
        public decimal RefundFee { get; set; }

        /// <summary>
        /// 编码:GBK或者UTF-8
        /// </summary>
        public String InputCharset { get; set; }
    }

    public class WeiXinRefundRequest : RefundRequest
    {
        /// <summary>
        /// 退款操作员账号
        /// </summary>
        public String Operator { get; set; }

        /// <summary>
        /// 退款操作员账号密码
        /// </summary>
        public String OperatorPwd { get; set; }

        /// <summary>
        /// 支付的总金额,单位分
        /// </summary>
        public Int32 PayAmount
        {
            get
            {
                return (Int32)(Fee * 100);
            }
        }

        /// <summary>
        /// 支付的总金额,单位分
        /// </summary>
        public Int32 RefundPayAmount
        {
            get
            {
                return (Int32)(RefundFee * 100);
            }
        }
    }

    public class AlipayRefundRequest : RefundRequest
    {

    }
}
