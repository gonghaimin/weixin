using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Enums
{
    //
    // 摘要:
    //     /// 当RequestMsgType类型为Event时，Event属性的类型 ///
    public enum Event
    {
        Unknown = -1,
        //
        // 摘要:
        //     /// 进入会话（似乎已从官方API中移除） ///
        ENTER,
        //
        // 摘要:
        //     /// 地理位置（似乎已从官方API中移除） ///
        LOCATION,
        //
        // 摘要:
        //     /// 订阅 ///
        subscribe,
        //
        // 摘要:
        //     /// 取消订阅 ///
        unsubscribe,
        //
        // 摘要:
        //     /// 自定义菜单点击事件 ///
        CLICK,
        //
        // 摘要:
        //     /// 二维码扫描 ///
        scan,
        //
        // 摘要:
        //     /// URL跳转 ///
        VIEW,
        //
        // 摘要:
        //     /// 事件推送群发结果 ///
        MASSSENDJOBFINISH,
        //
        // 摘要:
        //     /// 模板信息发送完成 ///
        TEMPLATESENDJOBFINISH,
        //
        // 摘要:
        //     /// 扫码推事件 ///
        scancode_push,
        //
        // 摘要:
        //     /// 扫码推事件且弹出“消息接收中”提示框 ///
        scancode_waitmsg,
        //
        // 摘要:
        //     /// 弹出系统拍照发图 ///
        pic_sysphoto,
        //
        // 摘要:
        //     /// 弹出拍照或者相册发图 ///
        pic_photo_or_album,
        //
        // 摘要:
        //     /// 弹出微信相册发图器 ///
        pic_weixin,
        //
        // 摘要:
        //     /// 弹出地理位置选择器 ///
        location_select,
        //
        // 摘要:
        //     /// 卡券通过审核 ///
        card_pass_check,
        //
        // 摘要:
        //     /// 卡券未通过审核 ///
        card_not_pass_check,
        //
        // 摘要:
        //     /// 领取卡券 ///
        user_get_card,
        //
        // 摘要:
        //     /// 删除卡券 ///
        user_del_card,
        //
        // 摘要:
        //     /// 多客服接入会话 ///
        kf_create_session,
        //
        // 摘要:
        //     /// 多客服关闭会话 ///
        kf_close_session,
        //
        // 摘要:
        //     /// 多客服转接会话 ///
        kf_switch_session,
        //
        // 摘要:
        //     /// 审核结果事件推送 ///
        poi_check_notify,
        //
        // 摘要:
        //     /// Wi-Fi连网成功 ///
        WifiConnected,
        //
        // 摘要:
        //     /// 卡券核销 ///
        user_consume_card,
        //
        // 摘要:
        //     /// 进入会员卡 ///
        user_view_card,
        //
        // 摘要:
        //     /// 从卡券进入公众号会话 ///
        user_enter_session_from_card,
        //
        // 摘要:
        //     /// 微小店订单付款通知 ///
        merchant_order,
        //
        // 摘要:
        //     /// 接收会员信息事件通知 ///
        submit_membercard_user_info,
        //
        // 摘要:
        //     /// 摇一摇事件通知 ///
        ShakearoundUserShake,
        //
        // 摘要:
        //     /// 卡券转赠事件推送 ///
        user_gifting_card,
        //
        // 摘要:
        //     /// 微信买单完成 ///
        user_pay_from_pay_cell,
        //
        // 摘要:
        //     /// 会员卡内容更新事件：会员卡积分余额发生变动时 ///
        update_member_card,
        //
        // 摘要:
        //     /// 卡券库存报警事件：当某个card_id的初始库存数大于200且当前库存小于等于100时 ///
        card_sku_remind,
        //
        // 摘要:
        //     /// 券点流水详情事件：当商户朋友的券券点发生变动时 ///
        card_pay_order,
        //
        // 摘要:
        //     /// 创建门店小程序审核事件 ///
        apply_merchant_audit_info,
        //
        // 摘要:
        //     /// 从腾讯地图中创建门店审核事件 ///
        create_map_poi_audit_info,
        //
        // 摘要:
        //     /// 门店小程序中创建门店审核事件 ///
        add_store_audit_info,
        //
        // 摘要:
        //     /// 修改门店图片审核事件 ///
        modify_store_audit_info,
        //
        // 摘要:
        //     /// 点击菜单跳转小程序的事件推送 ///
        view_miniprogram,
        //
        // 摘要:
        //     /// 资质认证成功（此时立即获得接口权限） ///
        qualification_verify_success,
        //
        // 摘要:
        //     /// 名称认证成功（即命名成功） ///
        qualification_verify_fail,
        //
        // 摘要:
        //     /// 名称认证成功（即命名成功） ///
        naming_verify_success,
        //
        // 摘要:
        //     /// 名称认证失败（这时虽然客户端不打勾，但仍有接口权限） ///
        naming_verify_fail,
        //
        // 摘要:
        //     /// 年审通知 ///
        annual_renew,
        //
        // 摘要:
        //     /// 认证过期失效通知 ///
        verify_expired,
        //
        // 摘要:
        //     /// 小程序审核成功 ///
        weapp_audit_success,
        //
        // 摘要:
        //     /// 小程序审核失败 ///
        weapp_audit_fail,
        //
        // 摘要:
        //     /// 用户购买礼品卡付款成功 ///
        giftcard_pay_done,
        //
        // 摘要:
        //     /// 用户购买后赠送 ///
        giftcard_send_to_friend,
        //
        // 摘要:
        //     /// 用户领取礼品卡成功 ///
        giftcard_user_accept
    }
}
