using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Pay.Models
{
    public class PayConfig
    {
        /// <summary>
        /// 商户身份标识
        /// </summary>
        public string PartnerId { get; set; }

        /// <summary>
        /// 商户密钥
        /// </summary>
        public string PartnerKey { get; set; }

        /// <summary>
        /// 是否支持退款
        /// </summary>
        public bool SupportRefund { get; set; }
    }


    public class WeiXinPayConfig : PayConfig
    {
        /// <summary>
        /// 公众号Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 代理机构的AppId
        /// </summary>
        public string AgentAppId { get; set; }

        /// <summary>
        /// 子商户的商户号
        /// </summary>
        public string SubPartnerId { get; set; }

        /// <summary>
        /// 公众账号接口密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 公众账号接口验证token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 公众号支付加密key
        /// </summary>
        public string PaySignKey { get; set; }

        /// <summary>
        /// 退款证书的绝对路径
        /// </summary>
        public string CertFilePath { get; set; }

        /// <summary>
        /// 微信支付的版本号V2或V3
        /// </summary>
        public string Version { get; set; }

        public bool IsMiniAppPay { get; set; }
    }

    public class AlipayPayConfig : PayConfig
    {
        /// <summary>
        /// 
        /// </summary>
        private string serverUrl = "https://openapi.alipay.com/gateway.do";

        private string signType = "RSA";

        private readonly string signType2 = "RSA2";

        /// <summary>
        /// 商家收款的支付宝账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 支付宝开放平台的ApppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 支付宝请求地址
        /// </summary>
        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }

        /// <summary>
        /// 支付验证地址
        /// </summary>
        public string MapiUrl { get; set; } = "https://mapi.alipay.com/gateway.do";

        /// <summary>
        /// 接口版本
        /// </summary>
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// 签名方式 当应用私钥length大于1000时表示为Sha256加密，否则为Sha1加密
        /// </summary>
        public string SignType
        {
            get { return MerchantPrivateKey != null && MerchantPrivateKey.Length > 1000 ? signType2 : signType; }
            set { signType = value; }
        }


        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string AlipayPublicKey { get; set; }

        /// <summary>
        /// 商家私钥
        /// </summary>
        public string MerchantPrivateKey { get; set; }

        /// <summary>
        /// 商家公钥
        /// </summary>
        public string MerchantPublicKey { get; set; }

        /// <summary>
        /// 服务商PID
        /// </summary>
        public string AgentPartnerId { get; set; }
    }

}
