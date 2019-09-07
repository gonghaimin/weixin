using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Utility
{
    public static class EnumUtility
    {
        public static T StringConvertToEnum<T>(this string str) where T:struct
        {
            T t;
            Enum.TryParse<T>(str,true, out t);
            return t;
        }
    }
}
