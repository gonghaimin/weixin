using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Weixin.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T AsEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string GetDisplayName(this Enum value)
        {
            var fieldName = value.ToString();
            var field = value.GetType().GetField(fieldName);

            if (field == null) return string.Empty;

            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute),false);
            if (attributes.Length > 0)
            {
                fieldName = (attributes[0] as DisplayAttribute).Name;
            }
            return fieldName;
        }

        public static IDictionary<string, string> GetDisplayNames(this Enum value)
        {
            var dictionary = new Dictionary<string, string>();
            var fields = value.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.IsSpecialName) continue;
                var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                var name = field.Name;
                if (attributes.Length > 0)
                {
                    name = (attributes[0] as DisplayAttribute).Name;
                }
                dictionary.Add(field.Name, name);
            }
            return dictionary;
        }

    }
}
