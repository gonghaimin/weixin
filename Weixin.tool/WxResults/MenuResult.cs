using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Models;

namespace Weixin.Tool.WxResults
{
    public class MenuResult : WxJsonResult
    {
        public MenuFull_ButtonGroup menu
        {
            get;
            set;
        }

        public List<MenuFull_ConditionalButtonGroup> conditionalmenu
        {
            get;
            set;
        }
    }
}
