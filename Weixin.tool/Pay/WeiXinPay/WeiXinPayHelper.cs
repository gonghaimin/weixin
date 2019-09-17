using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Pay.Models;

namespace Weixin.Tool.Pay.WeiXinPay
{
    internal class WeiXinPayHelper
    {
        private WeiXinPayConfig Config;
        private bool isV3 = false;
        private WeiXinPayHelperV3 wxPayV3;
        public WeiXinPayHelper(WeiXinPayConfig config)
        {
            Config = config;
            isV3 = Config.Version.Equals("V3", StringComparison.OrdinalIgnoreCase);
            if (isV3)
            {
                wxPayV3 = new WeiXinPayHelperV3(Config);
            }
        }

        /// <summary>
        /// 生成js api支付所需的参数或者扫码支付的url
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <returns></returns>
        public String PayAction(WeiXinPayRequest request)
        {
            if (isV3)
            {
                return wxPayV3.PayAction(request);
            }

            String package = CreatePackage(request);
            String timestamp = CommonUtil.GetTimestamp().ToString();
            String noncestr = CommonUtil.CreateNoncestr();
            String paySign = CreatePaySign(package, timestamp, noncestr, request.Productid);
            if (!String.IsNullOrEmpty(request.Productid))
            {
                return String.Format("weixin://wxpay/bizpayurl?sign={0}&appid={1}&productid={2}&timestamp={3}&noncestr={4}", paySign, Config.AppId, request.Productid, timestamp, noncestr);
            }

            Dictionary<string, string> jsapi = new Dictionary<string, string>();
            jsapi.Add("appId", Config.AppId);
            jsapi.Add("timeStamp", timestamp);
            jsapi.Add("nonceStr", noncestr);
            jsapi.Add("package", package);
            jsapi.Add("signType", "SHA1");
            jsapi.Add("paySign", paySign);

            var entries = jsapi.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        /// <summary>
        /// 扫码支付callback支付公司request
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public NativeResponse NativeCallbackRequest(HttpContext context)
        {
            if (isV3)
            {
                return wxPayV3.NativeCallbackRequest(context);
            }

            var result = new NativeResponse();

            SortedDictionary<String, String> xmlParams;
            SortedDictionary<String, String> param = CommonUtil.GetRequest(context, out xmlParams);

            var verify = IsSHA1Sign(xmlParams);
            //签名验证
            if (verify.Item1)
            {
                result.RetCode = "0";
                result.RetErrMsg = "ok";
                result.OpenId = verify.Item3;
                result.IsSubscribe = Int32.Parse(verify.Item4);
                result.ProductId = verify.Item2;
            }
            else
            {
                result.RetCode = "10001";
                result.RetErrMsg = "SHA1签名验证失败";
            }

            return result;

        }

        /// <summary>
        /// 扫码支付返回给支付公司信息
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <param name="retCode">返回值,0成功,其他已retmsg为准</param>
        /// <param name="retMsg">错误提示信息</param>
        /// <returns></returns>
        public String NativeCallbackResponse(WeiXinPayRequest request, String retCode, String retMsg)
        {
            if (isV3)
            {
                return wxPayV3.NativeCallbackResponse(request, retCode, retMsg);
            }

            String package = CreatePackage(request);
            String timestamp = CommonUtil.GetTimestamp().ToString();
            String noncestr = CommonUtil.CreateNoncestr();

            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            param.Add("appid", Config.AppId);
            param.Add("appkey", Config.PaySignKey);
            param.Add("package", package);
            param.Add("timestamp", timestamp);
            param.Add("noncestr", noncestr);
            param.Add("retcode", retCode);
            param.Add("reterrmsg", HttpUtility.UrlEncode(retMsg, System.Text.Encoding.GetEncoding("UTF-8")));
            String paySign = CreatePaySign(param);

            Dictionary<String, String> tmpParams = new Dictionary<String, String>();
            tmpParams.Add("AppId", Config.AppId);
            tmpParams.Add("Package", package);
            tmpParams.Add("TimeStamp", timestamp);
            tmpParams.Add("NonceStr", noncestr);
            tmpParams.Add("RetCode", retCode);
            tmpParams.Add("RetErrMsg", retMsg);
            tmpParams.Add("AppSignature", paySign);
            tmpParams.Add("SignMethod", "sha1");

            return CommonUtil.ArrayToXml(tmpParams);
        }

        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public WeiXinPayResponse PayNotify(HttpContext context)
        {
            if (isV3)
            {
                return wxPayV3.PayNotify(context);
            }

            SortedDictionary<String, String> xmlParams;
            SortedDictionary<String, String> param = CommonUtil.GetRequest(context, out xmlParams);
            var tmpParam = CommonUtil.FilterPara(param);
            if (!IsMD5Sign(tmpParam, param["sign"]))
            {
                return new WeiXinPayResponse
                {
                    RetCode = "10002",
                    RetMsg = "MD5签名验证失败"
                };
            }

            var sha1Verify = IsSHA1Sign(xmlParams);
            if (!sha1Verify.Item1)
            {
                return new WeiXinPayResponse
                {
                    RetCode = "10001",
                    RetMsg = "SHA1签名验证失败"
                };
            }

            WeiXinPayResponse result = new WeiXinPayResponse()
            {
                RetCode = "0",
                RetMsg = "ok",
                BankType = param.ContainsKey("bank_type") ? param["bank_type"] : "",
                TradeMode = param["trade_mode"],
                TradeState = param["trade_state"],
                PartnerId = param["partner"],
                PayAmount = decimal.Parse(param["total_fee"]) / 100,
                NotifyId = param["notify_id"],
                TransactionID = param["transaction_id"],
                OrderNO = param["out_trade_no"],
                IsSubscribe = sha1Verify.Item4,
                OpenId = sha1Verify.Item3,
                OriginalParams = param
            };

            result.Buyer = result.OpenId;

            if (param.ContainsKey("attach"))
            {
                result.Attach = param["attach"];
            }
            if (param.ContainsKey("bank_billno"))
            {
                result.BankBillno = param["bank_billno"];
            }

            String paytime = param["time_end"];
            paytime = paytime.Insert(4, "-");
            paytime = paytime.Insert(7, "-");
            paytime = paytime.Insert(10, " ");
            paytime = paytime.Insert(13, ":");
            paytime = paytime.Insert(16, ":");
            result.PayTime = DateTime.Parse(paytime);

            return result;

        }

        public WeiXinRefundResponse Refund(WeiXinRefundRequest request)
        {
            if (isV3)
            {
                return wxPayV3.Refund(request);
            }

            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            param.Add("input_charset", request.InputCharset);
            param.Add("partner", Config.PartnerId);
            if (!String.IsNullOrEmpty(request.OutTradeNO))
            {
                param.Add("out_trade_no", request.OutTradeNO);
            }
            if (!String.IsNullOrEmpty(request.TransactionID))
            {
                param.Add("transaction_id", request.TransactionID);
            }
            param.Add("out_refund_no", request.RefundNO);
            param.Add("total_fee", request.PayAmount.ToString());
            param.Add("refund_fee", request.RefundPayAmount.ToString());
            param.Add("op_user_id", request.Operator);
            param.Add("op_user_passwd", request.OperatorPwd);

            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey).ToLower();
            param.Add("sign", signValue);

            String tmpEncodePackageStr = CommonUtil.CreateLinkString(param, true, request.InputCharset);

            String requestUrl = TenpayConfig.RefundUrl + "?" + tmpEncodePackageStr;

            //通信
            TenpayHttpClient httpClient = new TenpayHttpClient();

            //应答
            ClientResponseHandler resHandler = new ClientResponseHandler();

            httpClient.setCertInfo(Config.CertFilePath, Config.PartnerId);
            //设置请求内容
            httpClient.setReqContent(requestUrl);
            //设置超时
            httpClient.setTimeOut(30);

            WeiXinRefundResponse res = new WeiXinRefundResponse();
            string rescontent = "";
            //后台调用
            if (httpClient.call())
            {
                //获取结果
                rescontent = httpClient.getResContent();

                resHandler.setKey(Config.PartnerKey);
                //设置结果参数
                resHandler.setContent(rescontent);

                //判断签名及结果
                if (resHandler.isTenpaySign())
                {
                    res.RetCode = resHandler.getParameter("retcode");
                    res.RetMsg = resHandler.getParameter("retmsg");

                    if (res.RetCode == "0")
                    {
                        res.OrderNO = resHandler.getParameter("out_trade_no");
                        res.TransactionID = resHandler.getParameter("transaction_id");
                        res.RefundNO = resHandler.getParameter("out_refund_no");
                        res.RefundId = resHandler.getParameter("refund_id");
                        res.RefundStatus = Int32.Parse(resHandler.getParameter("refund_status"));
                        res.RefundChannel = Int32.Parse(resHandler.getParameter("refund_channel"));
                        res.RefundAmount = decimal.Parse(resHandler.getParameter("refund_fee")) / 100;
                    }
                }
                else
                {
                    res.RetCode = "10001";
                    res.RetMsg = "验证签名失败";
                }

            }
            else
            {
                res.RetCode = "10000";
                res.RetMsg = "通信失败";
            }

            return res;
        }

        /// <summary>
        /// 发货通知
        /// </summary>
        /// <param name="openId">用户标识</param>
        /// <param name="transactionId">交易号</param>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        public Tuple<String, String> DeliverNotify(String openId, String transactionId, String orderId, String access_token = "")
        {
            try
            {
                String timestamp = CommonUtil.GetTimestamp().ToString();

                if (String.IsNullOrEmpty(access_token))
                {
                    access_token = new CommonApi(Config.AppId, Config.AppSecret).GetToken();
                }

                SortedDictionary<String, String> param = new SortedDictionary<String, String>();
                param.Add("appid", Config.AppId);
                param.Add("appkey", Config.PaySignKey);
                param.Add("openid", openId);
                param.Add("transid", transactionId);
                param.Add("out_trade_no", orderId);
                param.Add("deliver_timestamp", timestamp);
                param.Add("deliver_status", "1");
                param.Add("deliver_msg", "ok");
                String appSignature = CreatePaySign(param);

                var result = Custom.DeliverNotify(Config.AppId, openId, transactionId, orderId, timestamp, appSignature, access_token);
                return Tuple.Create<String, String>(((Int32)result.errcode).ToString(), result.errmsg);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 告警
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Tuple<String, String, String> Alarm(HttpContext context)
        {
            SortedDictionary<String, String> xmlParams;
            SortedDictionary<String, String> param = CommonUtil.GetRequest(context, out xmlParams);

            if (xmlParams.Count > 0)
            {
                String timeStamp = xmlParams["TimeStamp"];
                String errortype = xmlParams["ErrorType"];
                String description = xmlParams["Description"];
                String alarmcontent = xmlParams["AlarmContent"];
                String appSignature = xmlParams["AppSignature"];
                SortedDictionary<String, String> tmpParam = new SortedDictionary<String, String>();
                tmpParam.Add("appid", Config.AppId);
                tmpParam.Add("appkey", Config.PaySignKey);
                tmpParam.Add("errortype", errortype);
                tmpParam.Add("description", description);
                tmpParam.Add("alarmcontent", alarmcontent);
                tmpParam.Add("timestamp", timeStamp);

                if (appSignature == CreatePaySign(tmpParam))
                {
                    return Tuple.Create<String, String, String>(errortype, description, alarmcontent);
                }
            }

            return null;
        }

        /// <summary>
        /// 维权通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns>维权信息</returns>
        public FeedBack FeedBack(HttpContext context)
        {
            FeedBack feed = new FeedBack();
            if (context.Request.InputStream.Length == 0)
            {
                return null;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(context.Request.InputStream);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            XmlNodeList xnl = root.ChildNodes;
            String timestamp = "";
            String sign = "";

            foreach (XmlNode xnf in xnl)
            {

                switch (xnf.Name)
                {
                    case "OpenId":
                        feed.OpenId = xnf.InnerText; break;
                    case "TimeStamp":
                        timestamp = xnf.InnerText; break;
                    case "MsgType":
                        feed.MsgType = xnf.InnerText; break;
                    case "FeedbackId":
                        feed.FeedBackId = xnf.InnerText; break;
                    case "TransId":
                        feed.TransactionID = xnf.InnerText; break;
                    case "Reason":
                        feed.Reason = xnf.InnerText; break;
                    case "Solution":
                        feed.Solution = xnf.InnerText; break;
                    case "ExtInfo":
                        feed.ExtInfo = xnf.InnerText; break;
                    case "AppSignature":
                        sign = xnf.InnerText; break;
                    case "PicInfo":
                        var tmpItems = root.SelectNodes("PicInfo/item");
                        if (tmpItems != null)
                        {
                            for (int i = 0; i < tmpItems.Count; i++)
                            {
                                feed.PicInfo.Add(tmpItems[0].SelectSingleNode("PicUrl").InnerText);
                            }
                        }
                        break;
                }
            }

            SortedDictionary<String, String> tmpParam = new SortedDictionary<String, String>();
            tmpParam.Add("appid", Config.AppId);
            tmpParam.Add("appkey", Config.PaySignKey);
            tmpParam.Add("timestamp", timestamp);

            if (sign == CreatePaySign(tmpParam))
            {
                return feed;
            }

            return null;
        }

        /// <summary>
        /// 维权通知的处理结果
        /// </summary>
        /// <param name="openId">用户标识</param>
        /// <param name="feedBackId">投诉单号</param>
        /// <returns></returns>
        public Tuple<String, String> UpdateFeedBack(String openId, String feedBackId)
        {
            String access_token = new CommonApi(Config.AppId, Config.AppSecret).GetToken();

            var result = Custom.UpdateFeedBack(access_token, openId, feedBackId);
            return Tuple.Create<String, String>(((Int32)result.errcode).ToString(), result.errmsg);
        }

        public RedpackResponse SendRedpack(RedpackRequest request)
        {
            if (isV3)
            {
                return wxPayV3.SendRedpack(request);
            }
            return null;
        }

        public WeiXinPayResponse Micropay(WeiXinPayRequest request)
        {
            if (isV3)
            {
                return wxPayV3.Micropay(request);
            }
            return null;
        }

        /// <summary>
        /// 查询交易订单
        /// </summary>
        /// <returns></returns>
        public WeiXinPayResponse SearchOrder(PayOrderSearchRequest request)
        {
            if (isV3)
            {
                return wxPayV3.SearchOrder(request);
            }
            return null;
        }

        private bool IsMD5Sign(SortedDictionary<String, String> param, String sign)
        {
            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
            return signValue == sign;
        }

        private Tuple<Boolean, String, String, String> IsSHA1Sign(SortedDictionary<String, String> xmlParam)
        {
            String productId = "";
            if (xmlParam.Count > 0)
            {
                String timeStamp = xmlParam["TimeStamp"];
                String nonceStr = xmlParam["NonceStr"];
                String appSignature = xmlParam["AppSignature"];
                String openId = xmlParam["OpenId"];
                String isSubscribe = xmlParam["IsSubscribe"];

                SortedDictionary<String, String> param = new SortedDictionary<String, String>();
                param.Add("appid", Config.AppId);
                param.Add("appkey", Config.PaySignKey);
                productId = xmlParam.ContainsKey("ProductId") ? xmlParam["ProductId"] : "";
                if (!String.IsNullOrEmpty(productId))
                {
                    param.Add("productid", productId);
                }
                param.Add("timestamp", timeStamp);
                param.Add("noncestr", nonceStr);
                param.Add("openid", openId);
                param.Add("issubscribe", isSubscribe);

                return Tuple.Create<Boolean, String, String, String>(appSignature == CreatePaySign(param), productId, openId, isSubscribe);
            }

            return Tuple.Create<Boolean, String, String, String>(false, productId, "", "");

        }



        /// <summary>
        /// 生成支付签名
        /// </summary>
        /// <returns></returns>
        private String CreatePaySign(String package, String timestamp, String noncestr, String productid)
        {
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            param.Add("appid", Config.AppId);
            param.Add("timestamp", timestamp);
            param.Add("noncestr", noncestr);
            if (String.IsNullOrEmpty(productid))
            {
                param.Add("package", package);
            }
            else
            {
                param.Add("productid", productid);
            }
            param.Add("appkey", Config.PaySignKey);

            return CreatePaySign(param);
        }

        private String CreatePaySign(SortedDictionary<String, String> param)
        {
            String tmpStr = CommonUtil.CreateLinkString(param);
            return SHA1Util.Sha1(tmpStr);
        }

        /// <summary>
        /// 创建微信支付的package
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <returns></returns>
        private String CreatePackage(WeiXinPayRequest request)
        {
            SortedDictionary<String, String> param = CreatePayParams(request);
            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
            String tmpEncodePackageStr = CommonUtil.CreateLinkString(param, true, request.InputCharset);

            return tmpEncodePackageStr + "&sign=" + signValue;
        }

        private SortedDictionary<String, String> CreatePayParams(WeiXinPayRequest request)
        {
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            param.Add("bank_type", request.BankType);
            param.Add("body", request.ProductDesc);
            param.Add("partner", Config.PartnerId);
            param.Add("out_trade_no", request.OutTradeNO);
            param.Add("total_fee", request.PayAmount.ToString());
            param.Add("fee_type", "1");
            param.Add("notify_url", request.NotifyUrl);
            param.Add("spbill_create_ip", request.ClientIP);
            param.Add("input_charset", request.InputCharset);

            if (request.StartTime.HasValue && request.ExpireTime.HasValue)
            {
                param.Add("time_start", request.StartTime.Value.ToString("yyyyMMddHHmmss"));
                param.Add("time_expire", request.ExpireTime.Value.ToString("yyyyMMddHHmmss"));
            }

            if (!string.IsNullOrEmpty(request.Attach))
            {
                param.Add("attach", request.Attach);
            }

            return param;
        }
    }
}
