using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weixin.Tool.Pay.Handlers;
using Weixin.Tool.Pay.Models;
using Weixin.Tool.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weixin.webapi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class PayController : ControllerBase
    //{
    //    // GET: api/<controller>
    //    [HttpGet]
    //    public IEnumerable<string> Get()
    //    {
    //        return new string[] { "value1", "value2" };
    //    }
    //    [HttpPost]
    //    public string CreateWeChatPay()
    //    {
    //        var weiXinPayRequest = new WeiXinPayRequest
    //        {
    //            Productid ="",
    //            ClientIP = HttpContext.GetUserIp(),
    //            Fee = 12,
    //            InputCharset = "UTF-8",
    //            NotifyUrl = "",
    //            OutTradeNO ="",
    //            ProductDesc = "",
    //            Trade_type = WXTradeType.JSAPI,
    //            OpenId = "",
    //            IsMiniAppPay = true
    //        };
    //        //IPayHandler 开发时应通过依赖注入的方式注入
    //        PayConfig payConfig = new PayConfig();
    //        IPayHandler payHandler = PayHandler.GetInstance(payConfig);
    //        return payHandler.PayAction(weiXinPayRequest);
    //    }
    //    [HttpPost]
    //    public string WeChatNotify()
    //    {
    //        PayConfig payConfig = new PayConfig();
    //        IPayHandler payHandler = PayHandler.GetInstance(payConfig);
    //        var result = payHandler.PayNotify(HttpContext) as WeiXinPayResponse;
    //        return result.RetCode;
    //    }
    //}

}
