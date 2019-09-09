using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool
{
    public class ApiConfig
    {
        /// <summary>
        /// 获取token
        /// </summary>
        public static string tokenApi = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        /// <summary>
        /// 创建菜单
        /// </summary>
        public static string menucreate = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        /// <summary>
        /// 自定义菜单查询接口
        /// </summary>
        public static string menuget = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        /// <summary>
        /// 自定义菜单删除接口
        /// </summary>
        public static string menudelete = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        /// <summary>
        /// 创建个性化菜单
        /// </summary>
        public static string menuaddconditional = "https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}";
        /// <summary>
        /// 删除个性化菜单
        /// </summary>
        public static string menudelconditional = "https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token={0}";
        /// <summary>
        /// 获取自定义菜单配置接口
        /// </summary>
        public static string get_current_selfmenu_info = "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token={0}";
        /// <summary>
        /// 添加客服帐号
        /// post
        /// </summary>
        public static string kfaccountAdd = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token={0}";
        /// <summary>
        /// 修改客服帐号
        /// </summary>
        public static string kfaccountUpdate = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token={0}";
        /// <summary>
        /// 删除客服帐号
        /// </summary>
        public static string kfaccountDel = "https://api.weixin.qq.com/customservice/kfaccount/del?access_token={0}";
        /// <summary>
        /// 设置客服帐号的头像
        /// </summary>
        public static string kfaccountUploadImg = "http://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={0}&kf_account={1}";
        /// <summary>
        /// 获取所有客服账号
        /// </summary>
        public static string getkflist = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}";
        /// <summary>
        /// 客服接口-发消息
        /// </summary>
        public static string customSend = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        /// <summary>
        /// 客服输入状态
        /// </summary>
        public static string customTyping = "https://api.weixin.qq.com/cgi-bin/message/custom/typing?access_token={0}";
        /// <summary>
        /// 上传图文消息内的图片获取URL【订阅号与服务号认证后均可用】
        /// 请注意，本接口所上传的图片不占用公众号的素材库中图片数量的5000个的限制。图片仅支持jpg/png格式，大小必须在1MB以下。
        /// </summary>
        public static string uploadMedia = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}";
        /// <summary>
        /// 上传图文消息素材【订阅号与服务号认证后均可用】
        /// </summary>
        public static string uploadnews = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
        /// <summary>
        /// 根据标签进行群发【订阅号与服务号认证后均可用】
        /// </summary>
        public static string sendall = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
        /// <summary>
        /// 上传视频消息素材
        /// </summary>
        public static string uploadvideo = "https://api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token={0}";
        /// <summary>
        /// 根据OpenID列表群发【订阅号不可用，服务号认证后可用】
        /// </summary>
        public static string messageMassSend = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
        /// <summary>
        /// 删除群发【订阅号与服务号认证后均可用】
        /// </summary>
        public static string messageMassDel = "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}";
        /// <summary>
        /// 预览接口【订阅号与服务号认证后均可用】
        /// </summary>
        public static string messageMassPreview = "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}";
        /// <summary>
        /// 查询群发消息发送状态【订阅号与服务号认证后均可用】
        /// </summary>
        public static string messageMassGet = "https://api.weixin.qq.com/cgi-bin/message/mass/get?access_token={0}";
        /// <summary>
        /// 获取群发速度
        /// </summary>
        public static string messageMassSpeedGet = "https://api.weixin.qq.com/cgi-bin/message/mass/speed/get?access_token={0}";
        /// <summary>
        /// 设置群发速度
        /// </summary>
        public static string messageMassSpeedSet = "https://api.weixin.qq.com/cgi-bin/message/mass/speed/set?access_token={0}";
        /// <summary>
        /// 设置所属行业
        /// </summary>
        public static string api_set_industry = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        public static string get_industry = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token={0}";
        /// <summary>
        /// 获得模板ID
        /// </summary>
        public static string api_add_template = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
        /// <summary>
        /// 获取模板列表
        /// </summary>
        public static string get_all_private_template = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}";
        /// <summary>
        /// 删除模板
        /// </summary>
        public static string del_private_template = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token={0}";
        /// <summary>
        /// 发送模板消息
        /// </summary>
        public static string templateSend = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        /// <summary>
        /// 一次性订阅消息
        /// 第一步：需要用户同意授权，获取一次给用户推送一条订阅模板消息的机会
        /// </summary>
        public static string subscribemsg = "https://mp.weixin.qq.com/mp/subscribemsg?action=get_confirm&appid={0}&scene={1}&template_id={2}&redirect_url={3}&reserved={4}#wechat_redirect";
        /// <summary>
        /// 获取公众号的自动回复规则
        /// </summary>
        public static string get_current_autoreply_info = "https://api.weixin.qq.com/cgi-bin/get_current_autoreply_info?access_token={0}";
        /// <summary>
        /// 新增临时素材
        /// </summary>
        public static string mediaUpload = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
        /// <summary>
        /// 获取临时素材
        /// </summary>
        public static string mediaGet = "https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        public static string materialadd_news = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}";
        /// <summary>
        /// 上传图文消息内的图片获取URL
        /// </summary>
        public static string mediauploadimg = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}";
        /// <summary>
        /// 新增其他类型永久素材
        /// </summary>
        public static string materialadd_material = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}&type={1}";
        /// <summary>
        /// 获取永久素材
        /// </summary>
        public static string materialget_material = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}";
        /// <summary>
        /// 删除永久素材
        /// </summary>
        public static string materialdel_material = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}";
        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        public static string materialupdate_news = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={0}";
        /// <summary>
        /// 获取素材总数
        /// </summary>
        public static string materialget_materialcount = "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={1}";
        /// <summary>
        /// 获取素材列表
        /// </summary>
        public static string materialbatchget_material = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}";
        /// <summary>
        /// 删除回复（新增接口）
        /// </summary>
        public static string commentreplydelete = "https://api.weixin.qq.com/cgi-bin/comment/reply/delete?access_token={0}";
        /// <summary>
        ///  回复评论（新增接口）
        /// </summary>
        public static string commentreplyadd = "https://api.weixin.qq.com/cgi-bin/comment/reply/add?access_token={0}";
        /// <summary>
        /// 删除评论（新增接口）
        /// </summary>
        public static string commentdelete = "https://api.weixin.qq.com/cgi-bin/comment/delete?access_token={0}";
        /// <summary>
        /// 将评论取消精选
        /// </summary>
        public static string commentunmarkelect = "https://api.weixin.qq.com/cgi-bin/comment/unmarkelect?access_token={0}";
        /// <summary>
        /// 将评论标记精选（新增接口）
        /// </summary>
        public static string commentmarkelect = "https://api.weixin.qq.com/cgi-bin/comment/markelect?access_token={0}";
        /// <summary>
        /// 查看指定文章的评论数据（新增接口）
        /// </summary>
        public static string commentlist = "https://api.weixin.qq.com/cgi-bin/comment/list?access_token={0}";
        /// <summary>
        /// 关闭已群发文章评论（新增接口）
        /// </summary>
        public static string commentclose = "https://api.weixin.qq.com/cgi-bin/comment/close?access_token={0}";
        /// <summary>
        /// 打开已群发文章评论（新增接口）
        /// </summary>
        public static string commentopen = "https://api.weixin.qq.com/cgi-bin/comment/open?access_token={0}";
        /// <summary>
        /// 创建标签
        /// </summary>
        public static string tagscreate = "https://api.weixin.qq.com/cgi-bin/tags/create?access_token={0}";
        /// <summary>
        /// 获取公众号已创建的标签
        /// </summary>
        public static string tagsget = "https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}";
        /// <summary>
        /// 编辑标签
        /// </summary>
        public static string tagsupdate = "https://api.weixin.qq.com/cgi-bin/tags/update?access_token={0}";
        /// <summary>
        /// 删除标签
        /// </summary>
        public static string tagsdelete = "https://api.weixin.qq.com/cgi-bin/tags/delete?access_token={0}";
        /// <summary>
        ///  获取标签下粉丝列表
        /// </summary>
        public static string tagget = "https://api.weixin.qq.com/cgi-bin/user/tag/get?access_token={0}";
        /// <summary>
        /// 批量为用户打标签
        /// </summary>
        public static string tagsmembersbatchtagging = "https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}";
        /// <summary>
        /// 批量为用户取消标签
        /// </summary>
        public static string tagsmembersbatchuntagging = "https://api.weixin.qq.com/cgi-bin/tags/members/batchuntagging?access_token={0}";
        /// <summary>
        /// 获取用户身上的标签列表
        /// </summary>
        public static string tagsgetidlist = "https://api.weixin.qq.com/cgi-bin/tags/getidlist?access_token={0}";
        /// <summary>
        /// 设置用户备注名
        /// </summary>
        public static string userinfoupdateremark = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}";
        /// <summary>
        /// 获取用户基本信息（包括UnionID机制）
        /// </summary>
        public static string userinfo = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
        /// <summary>
        /// 批量获取用户基本信息
        /// </summary>
        public static string userinfobatchget = "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}";
        /// <summary>
        /// 获取用户列表
        /// </summary>
        public static string userget = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";
        /// <summary>
        /// 获取公众号的黑名单列表
        /// </summary>
        public static string tagsmembersgetblacklist = "https://api.weixin.qq.com/cgi-bin/tags/members/getblacklist?access_token={0}";
        /// <summary>
        /// 拉黑用户
        /// </summary>
        public static string tagsmembersbatchblacklist = "https://api.weixin.qq.com/cgi-bin/tags/members/batchblacklist?access_token={0}";
        /// <summary>
        /// 取消拉黑用户
        /// </summary>
        public static string tagsmembersbatchunblacklist = "https://api.weixin.qq.com/cgi-bin/tags/members/batchunblacklist?access_token={0}";

    }
}
