using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class PayRequest
    {
        /// <summary>
        /// 商品描述
        /// </summary>
        public string ProductDesc { get; set; }

        /// <summary>
        /// 支付附加数据,原样返回
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNO { get; set; }

        /// <summary>
        /// 订单总金额,单位元
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 支付通知结果Url
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 交易时的客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 交易开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 交易结束时间
        /// </summary>
        public DateTime? ExpireTime { get; set; }



        private String charset;

        /// <summary>
        /// 传入参数字符编码
        /// </summary>      
        public String InputCharset
        {
            get { return charset ?? "UTF-8"; }
            set { charset = value; }
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class WeiXinPayRequest : PayRequest
    {
        /// <summary>
        /// NATIVE原生支付的商品ID
        /// </summary>
        public String Productid { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public WXTradeType Trade_type { get; set; }

        /// <summary>
        /// 用户标识,JSAPI支付时必传
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 刷卡支付设备上授权码
        /// </summary>
        public String AuthCode { get; set; }

        /// <summary>
        /// 财付通的支付金额以分为单位
        /// </summary>
        public Int32 PayAmount
        {
            get
            {
                return (Int32)(Fee * 100);
            }
        }

        /// <summary>
        /// 是否是小程序支付
        /// </summary>
        public bool IsMiniAppPay { get; set; }
    }


    /// <summary>
    /// 微信支付模式
    /// </summary>
    public enum WXTradeType
    {
        JSAPI,
        /// <summary>
        /// 原生支付模式1:返回weixin://wxpay/bizpayurl
        /// </summary>
        PRENATIVE,
        /// <summary>
        /// 原生支付模式2:返回code_url
        /// </summary>
        NATIVE,
        APP,
        /// <summary>
        /// 刷卡支付
        /// </summary>
        Micropay
    }

    /// <summary>
    /// 查询订单
    /// </summary>
    public class PayOrderSearchRequest
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNO { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayType { get; set; }
    }

  
}

