using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Weixin.Tool.Messages.Base;
using Weixin.Tool.Messages.ResponseMessage;

namespace Weixin.Tool.Messages.MsgHelpers
{
    public static class EntityHelper
    {
        /// <summary>
        /// 根据XML信息填充实实体
        /// </summary>
        /// <typeparam name="T">MessageBase为基类的类型，Response和Request都可以</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="doc">XML</param>
        public static void FillEntityWithXml<T>(this T entity, XDocument doc) where T : class, new()
        {
            entity = (entity ?? new T());
            XElement root = doc.Root;
            if (root == null)
            {
                return;
            }
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                string name = propertyInfo.Name;
                if (root.Element(name) == null)
                {
                    continue;
                }
                switch (propertyInfo.PropertyType.Name)
                {
                    case "DateTime":
                    case "DateTimeOffset":
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Nullable`1":
                        EntityUtility.FillSystemType<T>(entity, propertyInfo, (IConvertible)root.Element(name).Value);
                        continue;
                    case "Boolean":
                        if (name == "FuncFlag")
                        {
                            EntityUtility.FillSystemType<T>(entity, propertyInfo, (IConvertible)(root.Element(name).Value == "1"));
                            continue;
                        }
                        break;
                    case "List`1":
                        {
                            Type[] genericArguments = propertyInfo.PropertyType.GetGenericArguments();
                            string name2 = genericArguments[0].Name;
                            if (name2 == "Article")
                            {
                                List<Article> list = new List<Article>();
                                foreach (XElement item in root.Element(name).Elements("item"))
                                {
                                    Article article = new Article();
                                    article.FillEntityWithXml(new XDocument(item));
                                    list.Add(article);
                                }
                                propertyInfo.SetValue(entity, list, null);
                            }
                            continue;
                        }
                    case "Music":
                        FillClassValue<Music>(entity, root, name, propertyInfo);
                        continue;
                    case "Image":
                        FillClassValue<Image>(entity, root, name, propertyInfo);
                        continue;
                    case "Voice":
                        FillClassValue<Voice>(entity, root, name, propertyInfo);
                        continue;
                    case "Video":
                        FillClassValue<Video>(entity, root, name, propertyInfo);
                        continue;
                    case "RequestMsgType":
                    case "ResponseMsgType":
                    case "Event":
                        continue;
                }
                bool flag = false;
                if (propertyInfo.PropertyType.IsEnum)
                {
                    try
                    {
                        propertyInfo.SetValue(entity, Enum.Parse(propertyInfo.PropertyType, root.Element(name).Value, ignoreCase: true), null);
                        flag = true;
                    }
                    catch
                    {
                    }
                }
                if (!flag)
                {
                    propertyInfo.SetValue(entity, root.Element(name).Value, null);
                }
            }
        }

        /// <summary>
        /// 填充复杂类型的参数
        /// </summary>
        /// <typeparam name="T">复杂类型</typeparam>
        /// <param name="entity">被填充实体</param>
        /// <param name="root">XML节点</param>
        /// <param name="childElementName">XML下一级节点的名称</param>
        /// <param name="prop">属性对象</param>
        public static void FillClassValue<T>(object entity, XElement root, string childElementName, PropertyInfo prop) where T : class, new()
        {
            T val = new T();
            FillEntityWithXml(val, new XDocument(root.Element(childElementName)));
            prop.SetValue(entity, val, null);
        }

        /// <summary>
        /// 将实体转为XML
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static XDocument ConvertEntityToXml<T>(this T entity) where T : class
        {
            //IL_000e: Unknown result type (might be due to invalid IL or missing references)
            if (entity == null)
            {
                throw new Exception("entity 参数不能为 null");
            }
            XDocument xDocument = new XDocument();
            xDocument.Add(new XElement("xml"));
            XElement root = xDocument.Root;
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                string name = propertyInfo.Name;
                if (name == "Articles")
                {
                    XElement xElement = new XElement("Articles");
                    foreach (Article item in propertyInfo.GetValue(entity, null) as List<Article>)
                    {
                        IEnumerable<XElement> content = ConvertEntityToXml(item).Root.Elements();
                        xElement.Add(new XElement("item", content));
                    }
                    root.Add(xElement);
                    continue;
                }
                if (name == "Music" || name == "Image" || name == "Video" || name == "Voice")
                {
                    XElement xElement3 = new XElement(name);
                    IEnumerable<XElement> content3 = ConvertEntityToXml(propertyInfo.GetValue(entity, null)).Root.Elements();
                    xElement3.Add(content3);
                    root.Add(xElement3);
                    continue;
                }
                switch (propertyInfo.PropertyType.Name)
                {
                    case "String":
                        root.Add(new XElement(name, new XCData((propertyInfo.GetValue(entity, null) as string) ?? "")));
                        continue;
                    case "DateTime":
                        root.Add(new XElement(name, DateTimeHelper.GetUnixDateTime((DateTime)propertyInfo.GetValue(entity, null))));
                        continue;
                    case "DateTimeOffset":
                        root.Add(new XElement(name, DateTimeHelper.GetUnixDateTime((DateTimeOffset)propertyInfo.GetValue(entity, null))));
                        continue;
                    case "Boolean":
                        if (name == "FuncFlag")
                        {
                            root.Add(new XElement(name, ((bool)propertyInfo.GetValue(entity, null)) ? "1" : "0"));
                            continue;
                        }
                        break;
                    case "ResponseMsgType":
                        root.Add(new XElement(name, new XCData(propertyInfo.GetValue(entity, null).ToString().ToLower())));
                        continue;
                    case "Article":
                        root.Add(new XElement(name, propertyInfo.GetValue(entity, null).ToString().ToLower()));
                        continue;
                }
                if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.IsPublic)
                {
                    IEnumerable<XElement> content5 = ConvertEntityToXml(propertyInfo.GetValue(entity, null)).Root.Elements();
                    root.Add(new XElement(name, content5));
                }
                else
                {
                    root.Add(new XElement(name, propertyInfo.GetValue(entity, null)));
                }
            }
            return xDocument;
        }

        /// <summary>
        /// 将实体转为XML字符串
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static string ConvertEntityToXmlString<T>(this T entity) where T : class
        {
            return ConvertEntityToXml(entity).ToString();
        }


        /// <summary>
        /// ResponseMessageBase.CreateFromRequestMessage&lt;T&gt;(requestMessage)的扩展方法
        /// </summary>
        /// <typeparam name="T">需要生成的ResponseMessage类型</typeparam>
        /// <param name="requestMessage">IRequestMessageBase接口下的接收信息类型</param>
        /// <returns></returns>
        public static T CreateResponseMessage<T>(this IRequestMessageBase requestMessage) where T : class, IResponseMessageBase
        {
            return ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage);
        }

    }
 }

