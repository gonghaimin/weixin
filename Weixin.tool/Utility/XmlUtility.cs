using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Yank.WeiXin.Robot.Utility
{
	class XmlUtility
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="json"></param>
		/// <param name="rootName"></param>
		/// <returns></returns>
		public static XDocument ParseJson(string json, string rootName)
		{
			return JsonConvert.DeserializeXNode(json, rootName);
		}
        ///<summary>
        /// XML序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string Serializa<T>(T className) where T:new()
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                xs.Serialize(writer, className);
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
        ///<summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlString) where T : new()
        {
            StringReader stringReader = new StringReader(xmlString);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T res = (T)xmlSerializer.Deserialize(stringReader);
            return res;
        }
    }
}
