using System;
using System.Collections.Generic;
using System.Text;

namespace CacheHelper
{
    public interface ICacheManager : IDisposable
    {
        T Get<T>(string key);
        void Set(string key, object data, int cacheSeconds);
        bool SetIfNotExist(string key, string data, int cacheSeconds);
        bool Exists(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();
    }
}
