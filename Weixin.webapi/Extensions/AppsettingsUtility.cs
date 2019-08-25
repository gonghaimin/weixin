using Microsoft.Extensions.Configuration;


namespace Weixin.WebApi.Extensions
{
    /// <summary>
    /// 读取json配置
    /// </summary>
    public class AppsettingsUtility
    {
        private static IConfiguration configuration;
        public AppsettingsUtility(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        /// <summary>
        /// 使用冒号来获取内层的配置项
        /// 例如：Logging:LogLevel；Logging:LogLevel:Default
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSetting(string key)
        {
            return configuration[key];
        }
    }
}
