using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weixin.WebApi.Policy
{
    //CommonAuthorize: IAuthorizationRequirement，这类主要是承载一些初始化值，然后传递到Handler中去，给验证做逻辑运算提供一些可靠的信息；自己根据自身情况自己定义适当的属性作为初始数据的承载容器；
    public class CommonAuthorize: IAuthorizationRequirement
    {
    }
}
