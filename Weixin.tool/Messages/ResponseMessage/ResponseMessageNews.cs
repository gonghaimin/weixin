using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Messages.Base;

namespace Weixin.Tool.Messages.ResponseMessage
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class ResponseMessageNews : ResponseMessageBase
    {
        protected override ResponseMsgType MsgType => ResponseMsgType.News;
        /// <summary>
        ///图文消息个数；当用户发送文本、图片、视频、图文、地理位置这五种消息时，开发者只能回复1条图文消息；其余场景最多可回复8条图文消息
        /// </summary>
        public int ArticleCount { get { return Articles!=null? Articles.Count():0; } set { } } 
        /// <summary>
        /// 图文消息信息，注意，如果图文数超过限制，则将只发限制内的条数
        /// </summary>
        public List<Article> Articles { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        public string Template
        {
            get
            {
                return $@"<xml>
                          <ToUserName><![CDATA[{0}]]></ToUserName>
                          <FromUserName><![CDATA[{1}]]></FromUserName>
                          <CreateTime>{2}</CreateTime>
                          <MsgType><![CDATA[{3}]]></MsgType>
                          <ArticleCount>{4}</ArticleCount>
                          <Articles>
                            {string.Join("", this.Articles.Select(item =>
                                {
                                    return $@" <item>
                                              < Title >< ![CDATA[{ item.Title}]]></Title>
                                              <Description><![CDATA[{item.Description}]]></Description>
                                              <PicUrl><![CDATA[{item.PicUrl}]]></PicUrl>
                                              <Url><![CDATA[{item.Url}]]></Url>
                                            </item>";
                                }))}
                           
                          </Articles>
                         <MsgId>{5}</MsgId>
                        </xml>";
            }
        }
        protected override string GenerateContent()
        {
            return string.Format(this.Template, this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType.ToString(), this.ArticleCount,this.MsgId);
        }
    }
    public class Article
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
    }
}
