using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages.Base
{
    /// <summary>
    /// 回复消息
    /// </summary>
    public abstract class ResponseMessageBase : MessageBase
    {
        protected string MsgId { get; set; }
        protected abstract ResponseMsgType MsgType { get; }
        /// <summary>
        /// 回复消息基本必填项校验
        /// </summary>
        /// <returns></returns>
        private bool VerifyBaseParmeter(out string msg)
        {
            msg = string.Empty;
            if (String.IsNullOrEmpty(this.ToUserName))
            {
                msg = "ToUserName";
                return false;
            }
            if (string.IsNullOrEmpty(this.FromUserName))
            {
                msg = "FromUserName";
                return false;
            }
            if(this.CreateTime <=0)
            {
                msg = "CreateTime";
                return false;
            }
            if ((int)this.MsgType == -1)
            {
                msg = "MsgType";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 回复消息其他必填项校验
        /// </summary>
        /// <returns></returns>
        protected virtual bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            return true;
        }
        /// <summary>
        /// 生成回复消息内容
        /// </summary>
        /// <returns>string</returns>
        protected abstract string GenerateContent();
        /// <summary>
        /// 获取响应用户消息
        /// </summary>
        /// <returns></returns>
        public string GetResponse(SignModel signModel=null)
        {
            string msg;
            if(VerifyBaseParmeter(out msg))
            {
                if (VerifyParameter(out msg))
                {
                    var response= GenerateContent();
                    if (signModel != null && !string.IsNullOrEmpty(signModel.msg_signature))
                    {
                        MsgCryptUtility mc = new MsgCryptUtility(WeiXinContext.Config.Token, WeiXinContext.Config.EncodingAESKey, WeiXinContext.Config.AppID);
                        var ret = mc.EncryptMsg(response, signModel.timestamp, signModel.nonce, ref response);
                        if(ret != 0)
                        {
                            throw new Exception("消息加密失败");
                        }
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(response);
                        XmlNode root = doc.FirstChild;
                        string sig = root["MsgSignature"].InnerText;
                        string enc = root["Encrypt"].InnerText;
                        string timestamp = root["TimeStamp"].InnerText;
                        string nonce = root["Nonce"].InnerText;
                        string stmp = "";
                        ret = mc.DecryptMsg(sig, timestamp, nonce, response, ref stmp);
                    }
                    return response;
                }
            }
            throw new Exception("缺少必填参数:" + msg);
        }
        /// <summary>
        ///  获取响应用户消息
        /// </summary>
        /// <param name="replyMessage">响应消息</param>
        /// <returns></returns>
        public string GetResponse(ResponseMessageBase message,SignModel signModel = null)
        {
            if(message.GetType() == this.GetType()){
                return this.GetResponse(signModel);
            }
            message.CreateTime = this.CreateTime;
            message.FromUserName = this.FromUserName;
            message.ToUserName = this.ToUserName;
            message.MsgId = this.MsgId;
            return message.GetResponse(signModel);
        }
        public static T CreateFromRequestMessage<T>(RequestMessageBase requestMessage) where T : ResponseMessageBase,new ()
        {
            T val = new T();
            val.FromUserName = requestMessage.ToUserName;
            val.ToUserName = requestMessage.FromUserName;
            val.CreateTime = Common.GetNowTime();
            val.MsgId = requestMessage.MsgId;
            return val;
        }
    }
}
