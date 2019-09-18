using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class PayResponse
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
        /// 是否支付成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return RetCode == "0" || RetCode == "SUCCESS";
            }
        }

        /// <summary>
        /// 支付结果状态码,0表示成功,其它为失败
        /// </summary>
        public String TradeState { get; set; }

        /// <summary>
        /// 交易模式：1-即时到账 其他保留
        /// </summary>
        public String TradeMode { get; set; }


        /// <summary>
        /// 银行类型
        /// </summary>
        public String BankType { get; set; }

        /// <summary>
        /// 支付金额:单位为元
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 银行订单号，若为余额支付则为空
        /// </summary>
        public String BankBillno { get; set; }

        /// <summary>
        /// 通知Id
        /// </summary>
        public String NotifyId { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public String TransactionID { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public String OrderNO { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 支付订单补充数据
        /// </summary>
        public String Attach { get; set; }

        /// <summary>
        /// 原始通知参数
        /// </summary>
        public SortedDictionary<String, String> OriginalParams { get; set; }

        /// <summary>
        /// 付款的账号,微信是openId,支付宝是支付宝账号
        /// </summary>
        public String Buyer { get; set; }

        public String BuyerId { get; set; }
    }

    public class WeiXinPayResponse : PayResponse
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public String PartnerId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 是否已订阅
        /// </summary>
        public String IsSubscribe { get; set; }
    }

    /// <summary>
    /// 扫码支付callback
    /// </summary>
    public class NativeResponse
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 用户是否订阅该公众帐号，1 为关注，0 为未关注
        /// </summary>
        public Int32 IsSubscribe { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public String ProductId { get; set; }

        /// <summary>
        /// 0表示成功,其他情况以RetErrMsg为准
        /// </summary>
        public String RetCode { get; set; }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        public String RetErrMsg { get; set; }
    }

  
}
