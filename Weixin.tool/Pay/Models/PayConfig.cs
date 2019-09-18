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

}
