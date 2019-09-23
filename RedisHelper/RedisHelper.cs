using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RedisHelper
{
    public class RedisHelper
    {
        private readonly RedisCacheOptions _options;
        private volatile ConnectionMultiplexer _connection;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private IDatabase _cache;
        private void Connect()
        {
            if (_cache == null)
            {
                _connectionLock.Wait();
                try
                {
                    if (_cache == null)
                    {
                        if (_options.ConfigurationOptions != null)
                        {
                            _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                        }
                        else
                        {
                            _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                        }
                        _cache = _connection.GetDatabase();
                    }
                }
                finally
                {
                    _connectionLock.Release();
                }
            }
        }
        private readonly string _instance;
        public RedisHelper(IOptions<RedisCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException("optionsAccessor");
            }
            _options = optionsAccessor.Value;
            _instance = (_options.InstanceName ?? string.Empty);
            Connect();
        }
        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            return _cache.StringSet(key, value);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return _cache.StringGet(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteKey(string key)
        {
            return _cache.KeyDelete(key);
        }
    }
}
