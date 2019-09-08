using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{

    public class MenuFull_ConditionalButtonGroup : MenuFull_ButtonGroup
    {
        public MenuMatchRule matchrule
        {
            get;
            set;
        }

        //
        // 摘要:
        //     /// 菜单Id，只在获取的时候自动填充，提交“菜单创建”请求时不需要设置 ///
        public long menuid
        {
            get;
            set;
        }
    }
}
