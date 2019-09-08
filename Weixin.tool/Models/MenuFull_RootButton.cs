using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Models
{
    public class MenuFull_RootButton
    {
        public string type
        {
            get;
            set;
        }

        public string key
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;
        }

        public string appid
        {
            get;
            set;
        }

        public string pagepath
        {
            get;
            set;
        }

        public string media_id
        {
            get;
            set;
        }

        public List<MenuFull_RootButton> sub_button
        {
            get;
            set;
        }
    }
}
