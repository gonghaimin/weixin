using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Pay.Models;
using Weixin.Tool.Pay.PayUtil;

namespace Weixin.Tool.Pay.WeiXinPay
{
    internal class WeiXinPayHelperV3
    {
        /// <summary>
        /// 统一支付的Url
        /// </summary>
        private static readonly string PayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 退款的Url
        /// </summary>
        private static readonly string RefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        /// <summary>
        /// 红包的Url
        /// </summary>
        private static readonly string RedpackUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

        /// <summary>
        /// 刷卡支付的Url
        /// </summary>
        private static readonly string MicropayUrl = "https://api.mch.weixin.qq.com/pay/micropay";

        /// <summary>
        /// 订单查询Url
        /// </summary>
        private static readonly string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        private static readonly String Success = "SUCCESS";

        private static readonly String Fail = "FAIL";

        private static readonly String Native1UrlFormat = "weixin://wxpay/bizpayurl?sign={0}&appid={1}&mch_id={2}&product_id={3}&time_stamp={4}&nonce_str={5}";

        private WeiXinPayConfig Config;
        private bool isSubCommercial;
        private string appid;
        public WeiXinPayHelperV3(WeiXinPayConfig config)
        {
            Config = config;
            isSubCommercial = !String.IsNullOrEmpty(config.SubPartnerId);
            appid = isSubCommercial ? config.AgentAppId : config.AppId;
        }

        /// <summary>
        /// 生成js api支付所需的参数或者扫码支付的url
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <returns></returns>
        public String PayAction(WeiXinPayRequest request)
        {
            try
            {
                String payparams = CreatePackage(request);
                if (request.Trade_type.Equals(WXTradeType.PRENATIVE))
                {
                    var tmpNative1Params = XmlToDic(payparams);
                    return String.Format(Native1UrlFormat, tmpNative1Params["sign"], appid, Config.PartnerId, request.Productid, tmpNative1Params["time_stamp"], tmpNative1Params["nonce_str"]);
                }


                String res = HttpRequestUtil.Send("POST", PayUrl, payparams);
                var tmpResult = XmlToDic(res);
                if (tmpResult["return_code"] == Fail)
                {
                    return tmpResult["return_msg"];
                }

                var tmpSignResult = CommonUtil.FilterPara(tmpResult);
                if (!IsMD5Sign(tmpSignResult, tmpResult["sign"]))
                {
                    return "签名验证失败";
                }

                if (tmpResult["result_code"] == Fail)
                {
                    var errMsg = "";
                    if (!tmpResult.TryGetValue("err_code_des", out errMsg))
                    {
                        errMsg = GetErrMsg(tmpResult["err_code"]);
                    }

                    return errMsg;
                }

                if (tmpResult["trade_type"].Equals(WXTradeType.JSAPI.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return JSApiPay(tmpResult["prepay_id"], request.IsMiniAppPay);
                }

                if (tmpResult["trade_type"].Equals(WXTradeType.NATIVE.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return NativePay(tmpResult["prepay_id"], tmpResult["code_url"]);
                }

                return "提交参数错误";
            }
            catch
            {
                return "提交参数错误";
            }
        }

        /// <summary>
        /// 获取jsapi支付的传输参数
        /// </summary>
        /// <param name="prepay_id">预支付ID</param>
        /// <returns></returns>
        private String JSApiPay(String prepay_id, bool isMiniAppPay = false)
        {
            String nonceStr = CommonUtil.CreateNoncestr();
            String timeStamp = CommonUtil.GetTimestamp().ToString();

            SortedDictionary<String, String> jsapi = new SortedDictionary<String, String>();
            jsapi.Add("appId", isMiniAppPay && isSubCommercial ? Config.AppId : appid);
            jsapi.Add("timeStamp", timeStamp);
            jsapi.Add("nonceStr", nonceStr);
            jsapi.Add("package", "prepay_id=" + prepay_id);
            jsapi.Add("signType", "MD5");

            string sign = CreatePaySign(jsapi);
            jsapi.Add("paySign", sign);

            var entries = jsapi.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        /// <summary>
        /// 获取原生支付的传输的参数
        /// </summary>
        /// <param name="prepay_id">预支付ID</param>
        /// <param name="code_url">支付的url</param>
        /// <returns></returns>
        private String NativePay(String prepay_id, String code_url)
        {
            SortedDictionary<String, String> native = new SortedDictionary<String, String>();
            native.Add("prepay_id", prepay_id);
            native.Add("code_url", code_url);

            var entries = native.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries.ToArray()) + "}";
        }

        /// <summary>
        /// 扫码支付callback支付公司request
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public NativeResponse NativeCallbackRequest(HttpContext context)
        {
            var result = new NativeResponse();

            SortedDictionary<String, String> xmlParams;
            SortedDictionary<String, String> param = CommonUtil.GetRequest(context, out xmlParams);
            var tmpSignResult = CommonUtil.FilterPara(param);
            if (!IsMD5Sign(tmpSignResult, param["sign"]))
            {
                result.RetCode = Fail;
                result.RetErrMsg = "签名验证失败";
            }

            result.RetCode = Success;
            result.RetErrMsg = "ok";
            result.OpenId = param.ContainsKey("openid") ? param["openid"] : param["sub_openid"];
            result.IsSubscribe = param["is_subscribe"] == "Y" ? 1 : 0;
            result.ProductId = param["product_id"];

            return result;
        }

        /// <summary>
        /// 扫码支付返回给支付公司信息
        /// </summary>
        /// <param name="request">支付请求实例</param>
        /// <param name="retCode">返回值,SUCCESS成功,其他已retmsg为准</param>
        /// <param name="retMsg">错误提示信息</param>
        /// <returns></returns>
        public String NativeCallbackResponse(WeiXinPayRequest request, String retCode, String retMsg)
        {
            SortedDictionary<String, String> tmpParams = new SortedDictionary<String, String>();
            tmpParams.Add("return_code", retCode);
            tmpParams.Add("return_msg", retMsg);

            if (retCode == Fail)
            {
                return CommonUtil.ArrayToXml(tmpParams);
            }

            request.Trade_type = WXTradeType.NATIVE;
            var tmpNativeRes = PayAction(request);
            if (!tmpNativeRes.Contains("prepay_id"))
            {
                tmpParams["return_code"] = Fail;
                tmpParams["return_msg"] = tmpNativeRes;
                return CommonUtil.ArrayToXml(tmpParams);
            }

            String noncestr = CommonUtil.CreateNoncestr();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var native = serializer.Deserialize<NativeRes>(tmpNativeRes);
            tmpParams.Add("appid", appid);
            tmpParams.Add("mch_id", Config.PartnerId);
            tmpParams.Add("nonce_str", noncestr);
            tmpParams.Add("prepay_id", native.prepay_id);
            tmpParams.Add("result_code", retCode);
            tmpParams.Add("err_code_des", retMsg);

            string sign = CreatePaySign(tmpParams);
            tmpParams.Add("sign", sign);

            return CommonUtil.ArrayToXml(tmpParams);
        }

        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="context">通知上下文</param>
        /// <returns></returns>
        public WeiXinPayResponse PayNotify(HttpContext context)
        {
            SortedDictionary<String, String> xmlParams;
            SortedDictionary<String, String> param = CommonUtil.GetRequest(context, out xmlParams);

            if (xmlParams["return_code"] == Fail)
            {
                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = xmlParams["return_msg"],
                    OriginalParams = xmlParams
                };
            }

            var tmpParam = CommonUtil.FilterPara(xmlParams);
            if (!IsMD5Sign(tmpParam, xmlParams["sign"]))
            {
                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = "MD5签名验证失败",
                    OriginalParams = xmlParams
                };
            }

            if (xmlParams["result_code"] == Fail)
            {
                var errMsg = "";
                if (!xmlParams.TryGetValue("err_code_des", out errMsg))
                {
                    errMsg = GetErrMsg(xmlParams["err_code"]);
                }

                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = errMsg,
                    OriginalParams = xmlParams
                };
            }

            var openid = xmlParams.ContainsKey("sub_openid") ? xmlParams["sub_openid"] : "";
            if (string.IsNullOrEmpty(openid))
            {
                openid = xmlParams.ContainsKey("openid") ? xmlParams["openid"] : "";
            }

            WeiXinPayResponse result = new WeiXinPayResponse()
            {
                RetCode = Success,
                RetMsg = "ok",
                BankType = xmlParams.ContainsKey("bank_type") ? xmlParams["bank_type"] : "",
                TradeMode = xmlParams["trade_type"],
                PartnerId = xmlParams["mch_id"],
                PayAmount = decimal.Parse(xmlParams["total_fee"]) / 100,
                TransactionID = xmlParams["transaction_id"],
                OrderNO = xmlParams["out_trade_no"],
                IsSubscribe = xmlParams["is_subscribe"] == "Y" ? "1" : "0",
                OpenId = openid,
                OriginalParams = xmlParams
            };

            result.Buyer = result.OpenId;

            if (xmlParams.ContainsKey("attach"))
            {
                result.Attach = xmlParams["attach"];
            }

            String paytime = xmlParams["time_end"];
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
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            String noncestr = CommonUtil.CreateNoncestr();
            param.Add("appid", appid);
            param.Add("mch_id", Config.PartnerId);
            param.Add("nonce_str", noncestr);
            if (isSubCommercial)
            {
                if (Config.IsMiniAppPay)
                {
                    param.Add("sub_appid", Config.AppId);
                }
                param.Add("sub_mch_id", Config.SubPartnerId);
            }
            param.Add("out_trade_no", request.OutTradeNO);
            param.Add("transaction_id", request.TransactionID);
            param.Add("out_refund_no", request.RefundNO);
            param.Add("total_fee", request.PayAmount.ToString());
            param.Add("refund_fee", request.RefundPayAmount.ToString());
            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey).ToLower();
            param.Add("sign", signValue);

            X509Certificate2 cert = new X509Certificate2(Config.CertFilePath, Config.PartnerId, X509KeyStorageFlags.MachineKeySet);

            String tmpRes = HttpRequestUtil.Send("POST", RefundUrl, CommonUtil.ArrayToXml(param), 60, cert);
            var tmpResult = XmlToDic(tmpRes);

            WeiXinRefundResponse res = new WeiXinRefundResponse();
            if (tmpResult["return_code"] == Fail)
            {
                return new WeiXinRefundResponse
                {
                    RetCode = Fail,
                    RetMsg = tmpResult["return_msg"]
                };
            }

            var tmpSignResult = CommonUtil.FilterPara(tmpResult);
            if (!IsMD5Sign(tmpSignResult, tmpResult["sign"]))
            {
                return new WeiXinRefundResponse
                {
                    RetCode = Fail,
                    RetMsg = "签名验证失败"
                };
            }

            if (tmpResult["result_code"] == Fail)
            {
                var errMsg = "";
                if (!tmpResult.TryGetValue("err_code_des", out errMsg))
                {
                    errMsg = GetErrMsg(tmpResult["err_code"]);
                }

                return new WeiXinRefundResponse
                {
                    RetCode = Fail,
                    RetMsg = errMsg
                };
            }

            return new WeiXinRefundResponse
            {
                RetCode = "0",
                RefundTime = DateTime.Now,
                OrderNO = tmpResult["out_trade_no"],
                TransactionID = tmpResult["transaction_id"],
                RefundNO = tmpResult["out_refund_no"],
                RefundId = tmpResult["refund_id"],
                RefundAmount = decimal.Parse(tmpResult["refund_fee"]) / 100
            };
        }

        public RedpackResponse SendRedpack(RedpackRequest request)
        {
            try
            {
                SortedDictionary<String, String> param = new SortedDictionary<String, String>();
                String noncestr = CommonUtil.CreateNoncestr();
                String timeStamp = CommonUtil.GetTimestamp().ToString();
                if (timeStamp.Length >= 10)
                {
                    timeStamp = timeStamp.Substring(timeStamp.Length - 10);
                }
                else
                {
                    timeStamp = timeStamp.PadLeft(10, '0');
                }
                var mchId = isSubCommercial ? Config.SubPartnerId : Config.PartnerId;
                string billno = mchId + DateTime.Now.ToString("yyyyMMdd") + timeStamp;
                param.Add("nonce_str", noncestr);
                param.Add("mch_billno", billno);
                param.Add("mch_id", mchId);
                param.Add("wxappid", appid);
                param.Add("send_name", request.SendName);
                param.Add("re_openid", request.OpenId);
                param.Add("total_amount", (request.Amount * 100).ToString());
                param.Add("total_num", "1");
                param.Add("wishing", request.Wishing);
                param.Add("client_ip", request.ClientIP);
                param.Add("act_name", request.ActivityName);
                param.Add("remark", request.Remark);
                String tmpPackageStr = CommonUtil.CreateLinkString(param);
                String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
                param.Add("sign", signValue);

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                X509Certificate cer = new X509Certificate(Config.CertFilePath, mchId, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

                String tmpRes = HttpRequestUtil.Send("POST", RedpackUrl, CommonUtil.ArrayToXml(param), 10, cer);
                var tmpResult = XmlToDic(tmpRes);
                if (tmpResult["return_code"] == Fail)
                {
                    return new RedpackResponse
                    {
                        RetCode = Fail,
                        RetMsg = tmpResult["return_msg"]
                    };
                }

                if (tmpResult["result_code"] == Fail)
                {
                    var errMsg = "";
                    if (!tmpResult.TryGetValue("err_code_des", out errMsg))
                    {
                        errMsg = GetErrMsg(tmpResult["err_code"]);
                    }

                    return new RedpackResponse
                    {
                        RetCode = Fail,
                        RetMsg = errMsg
                    };
                }

                return new RedpackResponse
                {
                    SendListId = tmpResult["send_listid"]
                };
            }
            catch (Exception ex)
            {
                return new RedpackResponse
                {
                    RetCode = Fail,
                    RetMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 刷卡支付
        /// </summary>
        /// <param name="request"></param>
        public WeiXinPayResponse Micropay(WeiXinPayRequest request)
        {
            try
            {
                SortedDictionary<String, String> param = new SortedDictionary<String, String>();
                String noncestr = CommonUtil.CreateNoncestr();
                String timeStamp = CommonUtil.GetTimestamp().ToString();
                if (timeStamp.Length >= 10)
                {
                    timeStamp = timeStamp.Substring(timeStamp.Length - 10);
                }
                else
                {
                    timeStamp = timeStamp.PadLeft(10, '0');
                }
                param.Add("appid", appid);
                if (isSubCommercial)
                {
                    if (Config.IsMiniAppPay)
                    {
                        param.Add("sub_appid", Config.AppId);
                    }
                    param.Add("sub_mch_id", Config.SubPartnerId);
                }
                param.Add("mch_id", Config.PartnerId);
                param.Add("nonce_str", noncestr);
                param.Add("body", request.ProductDesc);
                param.Add("out_trade_no", request.OutTradeNO);
                param.Add("total_fee", request.PayAmount.ToString());
                if (!string.IsNullOrEmpty(request.Attach))
                {
                    param.Add("attach", request.Attach);
                }
                param.Add("spbill_create_ip", request.ClientIP);
                param.Add("auth_code", request.AuthCode);
                String tmpPackageStr = CommonUtil.CreateLinkString(param);
                String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
                param.Add("sign", signValue);
                String tmpRes = HttpRequestUtil.Send("POST", MicropayUrl, CommonUtil.ArrayToXml(param), 600);

                return ConvertToPayResponse(tmpRes);

            }
            catch (Exception ex)
            {
                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 查询交易订单
        /// </summary>
        /// <returns></returns>
        public WeiXinPayResponse SearchOrder(PayOrderSearchRequest request)
        {
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            String noncestr = CommonUtil.CreateNoncestr();
            var mchId = isSubCommercial ? Config.SubPartnerId : Config.PartnerId;
            param.Add("appid", appid);
            if (isSubCommercial)
            {
                if (Config.IsMiniAppPay)
                {
                    param.Add("sub_appid", Config.AppId);
                }
                param.Add("sub_mch_id", Config.SubPartnerId);
            }
            param.Add("mch_id", Config.PartnerId);
            param.Add("nonce_str", noncestr);
            if (!string.IsNullOrEmpty(request.TransactionID))
            {
                param.Add("transaction_id", request.TransactionID);
            }

            if (!string.IsNullOrEmpty(request.OutTradeNO))
            {
                param.Add("out_trade_no", request.OutTradeNO);
            }

            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
            param.Add("sign", signValue);
            String tmpRes = HttpRequestUtil.Send("POST", OrderQueryUrl, CommonUtil.ArrayToXml(param));
            return ConvertToPayResponse(tmpRes);
        }

        private WeiXinPayResponse ConvertToPayResponse(string postResult)
        {
            var xmlParams = XmlToDic(postResult);
            if (xmlParams["return_code"] == Fail)
            {
                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = xmlParams["return_msg"]
                };
            }

            if (xmlParams["result_code"] == Fail)
            {
                return new WeiXinPayResponse
                {
                    RetCode = xmlParams["err_code"],
                    RetMsg = xmlParams["err_code_des"]
                };
            }

            if (xmlParams.ContainsKey("trade_state") && xmlParams["trade_state"] != "SUCCESS")
            {
                return new WeiXinPayResponse
                {
                    RetCode = xmlParams["trade_state"],
                    RetMsg = xmlParams["trade_state_desc"]
                };
            }

            var tmpParam = CommonUtil.FilterPara(xmlParams);
            if (!IsMD5Sign(tmpParam, xmlParams["sign"]))
            {
                return new WeiXinPayResponse
                {
                    RetCode = Fail,
                    RetMsg = "MD5签名验证失败",
                    OriginalParams = xmlParams
                };
            }

            var openid = xmlParams.ContainsKey("sub_openid") ? xmlParams["sub_openid"] : "";
            if (string.IsNullOrEmpty(openid))
            {
                openid = xmlParams.ContainsKey("openid") ? xmlParams["openid"] : "";
            }

            WeiXinPayResponse result = new WeiXinPayResponse()
            {
                RetCode = Success,
                RetMsg = "ok",
                BankType = xmlParams.ContainsKey("bank_type") ? xmlParams["bank_type"] : "",
                TradeMode = xmlParams.ContainsKey("trade_type") ? xmlParams["trade_type"] : "",
                PartnerId = xmlParams["mch_id"],
                PayAmount = xmlParams.ContainsKey("total_fee") ? decimal.Parse(xmlParams["total_fee"]) / 100 : 0,
                TransactionID = xmlParams["transaction_id"],
                OrderNO = xmlParams["out_trade_no"],
                OpenId = openid,
                OriginalParams = xmlParams,
            };

            result.Buyer = result.OpenId;

            if (xmlParams.ContainsKey("attach"))
            {
                result.Attach = xmlParams["attach"];
            }

            String paytime = xmlParams["time_end"];
            paytime = paytime.Insert(4, "-");
            paytime = paytime.Insert(7, "-");
            paytime = paytime.Insert(10, " ");
            paytime = paytime.Insert(13, ":");
            paytime = paytime.Insert(16, ":");
            result.PayTime = DateTime.Parse(paytime);

            return result;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        private bool IsMD5Sign(SortedDictionary<String, String> param, String sign)
        {
            String tmpPackageStr = CommonUtil.CreateLinkString(param);
            String signValue = MD5SignUtil.Sign(tmpPackageStr, Config.PartnerKey);
            return signValue == sign;
        }


        /// <summary>
        /// 生成支付签名
        /// </summary>
        /// <returns></returns>
        private String CreatePaySign(String package, String timestamp, String noncestr, String productid)
        {
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            param.Add("appid", appid);
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
            String signValue = MD5SignUtil.Sign(tmpStr, Config.PartnerKey);
            return signValue;
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
            param.Add("sign", signValue);

            return CommonUtil.ArrayToXml(param);
        }

        private SortedDictionary<String, String> CreatePayParams(WeiXinPayRequest request)
        {
            SortedDictionary<String, String> param = new SortedDictionary<String, String>();
            String noncestr = CommonUtil.CreateNoncestr();
            String timestamp = CommonUtil.GetTimestamp().ToString();
            param.Add("appid", appid);
            param.Add("mch_id", Config.PartnerId);
            param.Add("nonce_str", noncestr);
            if (isSubCommercial)
            {
                if (Config.IsMiniAppPay)
                {
                    param.Add("sub_appid", Config.AppId);
                }
                param.Add("sub_mch_id", Config.SubPartnerId);
            }

            switch (request.Trade_type)
            {
                case WXTradeType.JSAPI:
                    if (isSubCommercial && Config.IsMiniAppPay)
                    {
                        param.Add("sub_openid", request.OpenId);
                    }
                    else
                    {
                        param.Add("openid", request.OpenId);
                    }
                    break;
                case WXTradeType.PRENATIVE:
                    param.Add("time_stamp", timestamp);
                    param.Add("product_id", request.Productid);
                    break;
                case WXTradeType.NATIVE:
                    param.Add("product_id", request.Productid);
                    break;

            }

            if (!request.Trade_type.Equals(WXTradeType.PRENATIVE))
            {
                param.Add("body", request.ProductDesc);
                param.Add("out_trade_no", request.OutTradeNO);
                param.Add("total_fee", request.PayAmount.ToString());
                param.Add("spbill_create_ip", request.ClientIP);
                param.Add("notify_url", request.NotifyUrl);
                param.Add("trade_type", request.Trade_type.ToString());

                if (!String.IsNullOrEmpty(request.Attach))
                {
                    param.Add("attach", request.Attach);
                }
                if (request.StartTime.HasValue && request.ExpireTime.HasValue)
                {
                    param.Add("time_start", request.StartTime.Value.ToString("yyyyMMddHHmmss"));
                    param.Add("time_expire", request.ExpireTime.Value.ToString("yyyyMMddHHmmss"));
                }
            }

            return param;
        }

        private SortedDictionary<String, String> XmlToDic(String res)
        {
            SortedDictionary<String, String> result = new SortedDictionary<String, String>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(res);
            XmlNode root = xmlDoc.SelectSingleNode("xml");
            XmlNodeList xnl = root.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                result.Add(xnf.Name, xnf.InnerText);
            }

            return result;
        }

        private static String GetErrMsg(String errCode)
        {
            switch (errCode.ToUpper())
            {
                case "SYSTEMERROR":
                    return "接口后台错误";
                case "INVALID_TRANSACTIONID":
                    return "无效transaction_id";
                case "PARAM_ERROR":
                    return "提交参数错误";
                case "ORDERPAID":
                    return "订单已支付";
                case "OUT_TRADE_NO_USED":
                    return "商户订单号重复";
                case "NOAUTH":
                    return "商户无权限";
                case "NOTENOUGH":
                    return "余额不足";
                case "NOTSUPORTCARD":
                    return "不支持卡类型";
                case "ORDERCLOSED":
                    return "订单已关闭";
                case "BANKERROR":
                    return "银行系统异常";
                case "REFUND_FEE_INVALID":
                    return "退款金额大于支付金额";
                case "ORDERNOEXIST":
                    return "订单不存在";
                default:
                    return errCode;
            }


        }
    }

    internal class NativeRes
    {
        public String prepay_id { get; set; }

        public String code_url { get; set; }
    }
}