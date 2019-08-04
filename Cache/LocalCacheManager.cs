using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Cache
{
    
    public class LocalCacheManager : ICacheManager
    {


        private readonly IMemoryCache _cache;

        private static readonly object _syncRoot = new object();


        protected CancellationTokenSource _cancellationTokenSource;

        protected static readonly ConcurrentDictionary<string, bool> _allKeys;


        static LocalCacheManager()
        {
            _allKeys = new ConcurrentDictionary<string, bool>();
        }

        public LocalCacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _cancellationTokenSource = new CancellationTokenSource();
        }


        /// <summary>
        /// Create entry options to item of memory cache 
        /// </summary>
        /// <param name="cacheTime">Cache time in seconds</param>
        /// <returns></returns>
        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int cacheTime)
        {
            var options = new MemoryCacheEntryOptions()
                // add cancellation token for clear cache
                .AddExpirationToken(new CancellationChangeToken(_cancellationTokenSource.Token))
                //add post eviction callback
                .RegisterPostEvictionCallback(PostEviction);

            //set cache time
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTime);

            return options;
        }

        protected string AddKey(string key)
        {
            _allKeys.TryAdd(key, true);
            return key;
        }

        protected string RemoveKey(string key)
        {
            TryRemoveKey(key);
            return key;
        }

        protected void TryRemoveKey(string key)
        {
            //try to remove key from dictionary
            if (!_allKeys.TryRemove(key, out bool _))
                //if not possible to remove key from dictionary, then try to mark key as not existing in cache
                _allKeys.TryUpdate(key, false, false);
        }


        private void ClearKeys()
        {
            foreach (var key in _allKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                RemoveKey(key);
            }
        }

        /// <summary>
        /// Post eviction
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="value">Value of cached item</param>
        /// <param name="reason">Eviction reason</param>
        /// <param name="state">State</param>
        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            //if cached item just change, then nothing doing
            if (reason == EvictionReason.Replaced)
                return;

            //try to remove all keys marked as not existing
            ClearKeys();

            //try to remove this key from dictionary
            TryRemoveKey(key.ToString());
        }

 

        public virtual T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

 
        public virtual void Set(string key, object data, int cacheSeconds)
        {
            lock (_syncRoot)
            {
                if (data != null)
                {
                    _cache.Set(AddKey(key), data, GetMemoryCacheEntryOptions(cacheSeconds));
                }
            }
        }

        public bool SetIfNotExist(string key, string data, int cacheSeconds)
        {
            lock (_syncRoot)
            {
                if (Exists(key))
                {
                    return false;
                }
                else
                {
                    Set(key, data, cacheSeconds);
                    return true;
                }
            }
        }

        public bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue(key, out _);
        }

        public virtual void Remove(string key)
        {
            _cache.Remove(RemoveKey(key));
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matchesKeys = _allKeys.Where(p => p.Value).Where(p => p.Value && regex.IsMatch(p.Key)).ToList();
        }


        public virtual void Clear()
        {
            //send cancellation request
            _cancellationTokenSource.Cancel();

            //releases all resources used by this cancellation token
            _cancellationTokenSource.Dispose();

            //recreate cancellation token
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void Dispose()
        {
            //nothing special
        }

    }
}
