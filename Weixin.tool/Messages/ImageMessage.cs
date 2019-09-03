using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Messages
{
    /// <summary>
    /// 图片消息，可用于被动回复
    /// </summary>
    public class ImageMessage: IReplyMessage
    {
        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public string MediaId { get; set; }

        public ImageMessage()
        {
            this.MsgType = MsgTypeEnum.image.ToString();
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static ImageMessage LoadFromXml(string xml)
        {
            ImageMessage m = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    m = new ImageMessage();
                    m.FromUserName = element.Element(Common.FromUserName).Value;
                    m.ToUserName = element.Element(Common.ToUserName).Value;
                    m.CreateTime = element.Element(Common.CreateTime).Value;
                    m.MsgId = element.Element(Common.MsgId).Value;
                    m.PicUrl = element.Element("PicUrl").Value;
                    m.MediaId = element.Element(Common.MediaId).Value;
                }
            }

            return m;
        }
        /// <summary>
        /// 模板
        /// </summary>
        public string Template
        {
            get
            {
                return @"<xml>
                                  <ToUserName><![CDATA[{0}]]></ToUserName>
                                  <FromUserName><![CDATA[{1}]]></FromUserName>
                                  <CreateTime>{2}</CreateTime>
                                  <MsgType><![CDATA[{3}]]></MsgType>
                                   <Image>
                                    <MediaId><![CDATA[{4}]]></MediaId>
                                  </Image>
                            </xml>";
            }
        }
        protected override bool VerifyParameter(out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(this.MediaId))
            {
                msg = "MediaId";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 生成回复内容
        /// </summary>
        /// <returns></returns>
        protected override string GenerateContent()
        {
            this.CreateTime = Common.GetNowTime();
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, this.MediaId);
        }
    }
}
