﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Weixin.Tool.Enums
{
    public enum ReturnCode
    {
        未知错误=-2,
        系统繁忙此时请开发者稍候再试 = -1,
        请求成功 = 0,
        redirect_uri域名与后台配置不一致= 10003,
        此公众号被封禁= 10004,
        此公众号并没有这些scope的权限= 10005,
        必须关注此测试号= 10006,
        操作太频繁了,请稍后重试= 10009,
        scope不能为空= 10010,
        redirect_uri不能为空= 10011,
        appid不能为空= 10012,
        state不能为空= 10013,
        公众号未授权第三方平台,请检查授权状态= 10015,
        不支持微信开放平台的Appid,请使用公众号Appid= 10016,
        获取access_token时AppSecret错误或者access_token无效 = 40001,
        不合法的凭证类型 = 40002,
        不合法的OpenID = 40003,
        不合法的媒体文件类型 = 40004,
        不合法的EncodingAESKey = 40005,
        不合法的media_id = 40007,
        不合法的message_type = 40008,
        不合法的图片文件大小 = 40009,
        不合法的语音文件大小 = 40010,
        不合法的视频文件大小 = 40011,
        不合法的缩略图文件大小 = 40012,
        不合法的APPID = 40013,
        不合法的access_token = 40014,
        不合法的菜单类型 = 40015,
        不合法的菜单按钮个数 = 40016,
        不合法的按钮类型 = 40017,
        不合法的按钮名字长度 = 40018,
        不合法的按钮KEY长度 = 40019,
        不合法的按钮URL长度 = 40020,
        不合法的子菜单按钮个数 = 40023,
        不合法的子菜单按钮类型 = 40024,
        不合法的子菜单按钮名字长度 = 40025,
        不合法的子菜单按钮KEY长度 = 40026,
        不合法的子菜单按钮URL长度 = 40027,
        不合法或已过期的code = 40029,
        不合法的refresh_token = 40030,
        不合法的template_id长度=40036,
        不合法的template_id = 40037,
        不合法的URL长度 = 40039,
        不合法的url域名=40048,
        不合法的子菜单按钮url域名 = 40054,
        不合法的菜单按钮url域名 = 40055,
        不合法的url = 40066,
        缺少access_token参数 = 41001,
        缺少appid参数 = 41002,
        缺少refresh_token参数 = 41003,
        缺少secret参数 = 41004,
        缺失二进制媒体文件 = 41005,
        缺少media_id参数 = 41006,
        缺少子菜单数据 = 41007,
        缺少oauth_code = 41008,
        缺少openid = 41009,
        缺失url参数=41010,
        access_token超时 = 42001,
        refresh_token超时 = 42002,
        oauth_code超时 = 42003,
        需要GET请求 = 43001,
        需要POST请求 = 43002,
        需要HTTPS请求 = 43003,
        需要订阅关系 = 43004,
        多媒体文件为空 = 44001,
        POST的数据包为空 = 44002,
        图文消息内容为空 = 44003,
        文本消息内容为空 = 44004,
        多媒体文件大小超过限制 = 45001,
        content参数超过限制 = 45002,
        title参数超过限制 = 45003,
        description参数超过限制 = 45004,
        url参数长度超过限制 = 45005,
        picurl参数超过限制 = 45006,
        播放时间超过限制,语音为60s最大 = 45007,
        article参数超过限制 = 45008,
        接口调动频率超过限制 = 45009,
        创建菜单个数超过限制 = 45010,
        api频率限制=45011,
        模板大小超过限制=45012,
        不能修改默认组 = 45016,
        分组名字过长 = 45017,
        分组数量超过上限 = 45018,
        command字段取值不对=45072,
        下发输入状态,需要之前30秒内跟用户有过消息交互=45080,
        已经在输入状态,不可重复下发=45081,
        设置的speed参数不在0到4的范围内=45083,
        没有设置speed参数=45084,
        接口未授权 =50001,
        第三方不具备调用该接口的权限=61007,

        //#自定义code#
        xml解析失败=70000,
        Encrypt解密失败 = 70001,
        Encrypt加密失败 = 70002,
        msg_signature验签失败=70003,
        msg_signature生成失败=70004,

    }
}
