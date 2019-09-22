using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Pay.Models;
using Weixin.Tool.Pay.WeiXinPay;

namespace Weixin.Tool.Pay.Handlers
{
    public class WeiXinHandler : PayHandler
    {
        private WeiXinPayHelper weixin;
        public WeiXinHandler()
        {
            var tmpConfig = Config as WeiXinPayConfig;
            weixin = new WeiXinPayHelper(tmpConfig);
        }

        public override string PayAction(PayRequest request)
        {
            WeiXinPayRequest tmpRequest = request as WeiXinPayRequest;
            return weixin.PayAction(tmpRequest);
        }

        public override PayResponse PayNotify(HttpContext context)
        {
            return weixin.PayNotify(context);
        }

        public override NativeResponse NativeCallbackRequest(HttpContext context)
        {
            return weixin.NativeCallbackRequest(context);
        }

        public override string NativeCallbackResponse(PayRequest request, string retCode, string retMsg)
        {
            WeiXinPayRequest tmp = request as WeiXinPayRequest;
            return weixin.NativeCallbackResponse(tmp, retCode, retMsg);
        }

        public override RefundResponse Refund(RefundRequest request)
        {
            WeiXinRefundRequest tmp = request as WeiXinRefundRequest;
            return weixin.Refund(tmp);
        }


        public override RedpackResponse SendRedpack(RedpackRequest request)
        {
            return weixin.SendRedpack(request);
        }

        public override PayResponse Micropay(PayRequest request)
        {
            WeiXinPayRequest tmpRequest = request as WeiXinPayRequest;
            return weixin.Micropay(tmpRequest);
        }

        /// <summary>
        /// 查询交易订单
        /// </summary>
        /// <returns></returns>
        public override PayResponse OrderQuery(PayOrderSearchRequest request)
        {
            return weixin.OrderQuery(request);
        }

    }
}

