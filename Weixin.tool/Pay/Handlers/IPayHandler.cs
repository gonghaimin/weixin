using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Pay.Models;

namespace Weixin.Tool.Pay.Handlers
{
    public interface IPayHandler
    {
        /// <summary>
        /// 请求支付
        /// </summary>
        /// <param name="request">支付基本参数</param>
        /// <returns>返回支付的地址,微信支付的时候返回JS所需的Json参数</returns>
        String PayAction(PayRequest request);

        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="context">通知的上下文</param>
        /// <returns></returns>
        PayResponse PayNotify(HttpContext context);

        /// <summary>
        /// 扫码支付callback支付公司request
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        NativeResponse NativeCallbackRequest(HttpContext context);


        /// <summary>
        /// 扫码支付返回给支付公司信息
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <param name="retCode">返回值,0成功,其他已retmsg为准</param>
        /// <param name="retMsg">错误提示信息</param>
        /// <returns></returns>
        String NativeCallbackResponse(PayRequest request, String retCode, String retMsg);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="request">退款请求</param>
        RefundResponse Refund(RefundRequest request);


        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <param name="request">红包请求类</param>
        /// <returns></returns>
        RedpackResponse SendRedpack(RedpackRequest request);

        /// <summary>
        /// 刷卡支付
        /// </summary>
        PayResponse Micropay(PayRequest request);

        /// <summary>
        /// 查询交易订单
        /// </summary>
        /// <returns></returns>
        PayResponse SearchOrder(PayOrderSearchRequest request);


    }

    public abstract class PayHandler : IPayHandler
    {
        protected static PayConfig Config { get; private set; }

        /// <summary>
        /// 初始化支付实例
        /// </summary>
        /// <param name="config">支付方式等支付所需的基本信息</param>
        /// <returns></returns>
        public static PayHandler GetInstance(PayConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("支付基本参数为空");
            }

            PayHandler instance = null;
            Config = config;
            String configTypeName = config.GetType().Name;
            switch (configTypeName)
            {
                case "WeiXinPayConfig":
                    instance = new WeiXinHandler();
                    break;
                default:
                    throw new ArgumentNullException("支付基本参数为空");
            }

            return instance;
        }

        public abstract String PayAction(PayRequest request);

        public abstract PayResponse PayNotify(HttpContext context);

        public abstract NativeResponse NativeCallbackRequest(HttpContext context);

        public abstract String NativeCallbackResponse(PayRequest request, String retCode, String retMsg);

        public abstract RefundResponse Refund(RefundRequest request);

        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <param name="request">红包请求类</param>
        /// <returns></returns>
        public abstract RedpackResponse SendRedpack(RedpackRequest request);

        /// <summary>
        /// 刷卡支付
        /// </summary>
        public abstract PayResponse Micropay(PayRequest request);


        /// <summary>
        /// 查询交易订单
        /// </summary>
        /// <returns></returns>
        public abstract PayResponse SearchOrder(PayOrderSearchRequest request);


    
    }
}
