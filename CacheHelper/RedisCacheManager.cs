using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Text;


namespace CacheHelper
{
    
    public class RedisCacheManager : ICacheManager
    {
        private static readonly RedisCache Cache = new RedisCache(
            new RedisCacheOptions() { InstanceName="Redis", Configuration="172.0.0.1:7963" });

        public void Dispose()
        {
            Cache.Dispose();
        }

        public bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            var val = Cache.Get(key);
            return val !=null;
        }

        public T Get<T>(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            var val = Cache.GetString(key);
            if(!string.IsNullOrEmpty(val))
            {
               return JsonConvert.DeserializeObject<T>(val);
            }
            return default(T);
        }

        public void Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {

        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <returns></returns>
        public void Set(string key, object value, int cacheSeconds)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var str = JsonConvert.SerializeObject(value);
            Cache.SetString(key, str, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow=TimeSpan.FromSeconds(cacheSeconds) });

        }

        public bool SetIfNotExist(string key, string data, int cacheSeconds)
        {
            var val = Cache.Get(key);
            if (val == null)
            {
                Set(key, data, cacheSeconds);
            }
            return true;
        }
    }
}
