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
    public class ResponseMessageNews : ResponseMessageBase, IResponseMessageBase, IMessageBase
    {
        public override ResponseMsgType MsgType => ResponseMsgType.News;

        public int ArticleCount
        {
            get
            {
                if (Articles != null)
                {
                    return Articles.Count;
                }
                return 0;
            }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        public List<Article> Articles
        {
            get;
            set;
        }

        public ResponseMessageNews()
        {
            Articles = new List<Article>();
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
