using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Xml;

namespace Weixin.Tool.Utility
{
    public class WeatherHelper
    {
        /// <summary>
        /// 城市集合字段
        /// </summary>
        private static Dictionary<string, City> mCities;
      
		/// <summary>
        /// 城市集合
        /// </summary>
		public static Dictionary<string, City> Cities
        {
            get
            {
                if (mCities == null)
                {
                    LoadCities();
                }

                return mCities;
            }
        }
       
		/// <summary>
        /// 加载城市
        /// </summary>
        private static void LoadCities()
        {
            mCities = new Dictionary<string, City>();
            mCities.Clear();
            mCities.Add("101010100", new City() { Code = "101010100", Name = "北京", PinYin = "beijing", FristLetter = "bj" });
            mCities.Add("101020100", new City() { Code = "101020100", Name = "上海", PinYin = "shanghai", FristLetter = "sh" });
            mCities.Add("101200101", new City() { Code = "101200101", Name = "武汉", PinYin = "wuhai", FristLetter = "wh" });
            
        }
     
		/// <summary>
        /// 获取城市的天气
        /// </summary>
        /// <param name="name">城市名称、拼音或首字母</param>
        /// <returns></returns>
        public static string GetWeather(string name)
        {
            string result = string.Empty;
            string cityCode = string.Empty;
            //获取城市编码
            IEnumerable<string> codes = from item in Cities
                                        where item.Value != null
                                              && (item.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase) 
                                                    || item.Value.PinYin.Equals(name, StringComparison.OrdinalIgnoreCase) 
                                                    || item.Value.FristLetter.Equals(name, StringComparison.OrdinalIgnoreCase))
                                        select item.Value.Code;
            if (codes != null && codes.Count() > 0)
            {
                cityCode = codes.First<string>();
            }

            //http请求，获取天气
            if (!string.IsNullOrEmpty(cityCode))
            {
				string url = "http://m.weather.com.cn/mweather/{0}.shtml";
                url = string.Format(url, cityCode);
                WebRequest request = HttpWebRequest.Create(url);
                //超时时间为：2秒
                request.Timeout = 20000;
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string weahterInfo = reader.ReadToEnd();
                if (string.IsNullOrEmpty(weahterInfo))
                {
                    result = "暂时没有取到天气数据,请稍后再试";
                }
                else
                {
					try
					{
						XmlDocument doc = JsonConvert.DeserializeXmlNode(weahterInfo);
						if (doc != null)
						{
							XmlNode node = doc.DocumentElement;
							if (node != null)
							{
								StringBuilder builder = new StringBuilder();
								builder.Append(node["city"].InnerText).Append("\n");
								builder.Append(node["date_y"].InnerText).Append(" ").Append(node["week"].InnerText).Append("\n");
								builder.Append("今天：").Append("(").Append(node["temp1"].InnerText).Append(")").Append(node["weather1"].InnerText).Append(node["wind1"].InnerText).Append(node["fl1"].InnerText).Append("\n");
								builder.Append("24小时穿衣指数:").Append(node["index_d"].InnerText).Append("\n");
								builder.Append("明天：").Append("(").Append(node["temp2"].InnerText).Append(")").Append(node["weather2"].InnerText).Append(node["wind2"].InnerText).Append(node["fl2"].InnerText).Append("\n");
								builder.Append("48小时穿衣指数:").Append(node["index48_d"].InnerText).Append("\n");
								result = builder.ToString();
							}
						}
					}
					catch 
					{

						throw (new ArgumentNullException());
					}
                }
            }
            else
            {
                result = "没有获取到该城市的天气,请确定输入了正确的城市名称,如\'北京\'或者\'beijing\'或者\'bj\'";
            }
            //返回
            return result;
        }
        /// <summary>
        /// 内部类：城市
        /// </summary>
        public class City
        {
            /// <summary>
            /// 编码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 拼音
            /// </summary>
            public string PinYin { get; set; }
            /// <summary>
            /// 拼音首字母
            /// </summary>
            public string FristLetter { get; set; }
        }
    }
}
