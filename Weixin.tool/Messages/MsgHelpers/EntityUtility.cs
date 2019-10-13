using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Weixin.Tool.Messages.MsgHelpers
{
    public static class EntityUtility
    {
        /// <summary>
        /// 将对象转换到指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (convertibleValue == null)
            {
                return default(T);
            }
            Type type = typeof(T);
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() != typeof(Nullable<>))
                {
                    throw new InvalidCastException($"Invalid cast from type \"{convertibleValue.GetType().FullName}\" to type \"{typeof(T).FullName}\".");
                }
                type = Nullable.GetUnderlyingType(type);
            }
            return (T)Convert.ChangeType(convertibleValue, type);
        }

        /// <summary>
        /// 向属性填充值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void FillSystemType<T>(T entity, PropertyInfo prop, IConvertible value)
        {
            FillSystemType(entity, prop, value, prop.PropertyType);
        }

        /// <summary>
        /// 向属性填充值（强制使用指定的类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        /// <param name="specialType"></param>
        public static void FillSystemType<T>(T entity, PropertyInfo prop, IConvertible value, Type specialType)
        {
            object obj = null;
            if (value.GetType() != specialType)
            {
                switch (specialType.Name)
                {
                    case "Boolean":
                        obj = ConvertTo<bool>(value);
                        break;
                    case "DateTime":
                        obj = DateTimeHelper.GetDateTimeFromXml(value.ToString());
                        break;
                    case "DateTimeOffset":
                        obj = DateTimeHelper.GetDateTimeOffsetFromXml(value.ToString());
                        break;
                    case "Int32":
                        obj = ConvertTo<int>(value);
                        break;
                    case "Int32[]":
                        obj = Array.ConvertAll(ConvertTo<string>(value).Split(','), int.Parse);
                        break;
                    case "Int64":
                        obj = ConvertTo<long>(value);
                        break;
                    case "Int64[]":
                        obj = Array.ConvertAll(ConvertTo<string>(value).Split(','), long.Parse);
                        break;
                    case "Double":
                        obj = ConvertTo<double>(value);
                        break;
                    case "String":
                        obj = value.ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        obj = value;
                        break;
                }
            }
            string name = specialType.Name;
            if (name == "Nullable`1")
            {
                if (!string.IsNullOrEmpty(value as string))
                {
                    Type[] genericArguments = prop.PropertyType.GetGenericArguments();
                    FillSystemType(entity, prop, value, genericArguments[0]);
                }
                else
                {
                    prop.SetValue(entity, null, null);
                }
            }
            else
            {
                prop.SetValue(entity, obj ?? value, null);
            }
        }
    }
}
