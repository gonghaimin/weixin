using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class RefundResponse
    {
        /// <summary>
        /// 返回状态码，0表示成功，其它未定义
        /// </summary>
        public String RetCode { get; set; }

        /// <summary>
        /// 返回信息，如非空，为错误原因
        /// </summary>
        public String RetMsg { get; set; }

        /// <summary>
        /// 支付金额:单位为元
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款Id
        /// </summary>
        public String RefundId { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public String TransactionID { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        public String RefundNO { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public String OrderNO { get; set; }

        public DateTime RefundTime { get; set; }

        /// <summary>
        /// 原始通知参数
        /// </summary>
        public SortedDictionary<String, String> OriginalParams { get; set; }

        /// <summary>
        /// 接口返回的原始数据
        /// </summary>
        public string OriginalString { get; set; }
    }

    public class WeiXinRefundResponse : RefundResponse
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public String PartnerId { get; set; }

        /// <summary>
        /// 退款渠道,0:退到财付通、1:退到银行
        /// </summary>
        public Int32 RefundChannel { get; set; }

        /// <summary>
        /// 4，10：退款成功。
        /// 3，5，6：退款失败。
        /// 8，9，11：退款处理中。
        /// 1，2：未确定，需要商户原退款单号重新发起。
        /// 7：转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者财付通转账的方式进行退款。
        /// </summary>
        public Int32 RefundStatus { get; set; }

    }



}
