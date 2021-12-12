using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CacheStorage.StackExchangeRedis.Helper
{
    public class DistributeCache : IRedisDestributeCache
    {
        //private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public DistributeCache(IDistributedCache cache, IConfiguration config)
        {
            _distributedCache = cache;
            _configuration = config;
            //_distributedCacheEntryOptions 
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            //    SlidingExpiration = null;
            //};
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException || ex is RedisServerException || ex is RedisException)
            {
                //logging
            }
        }

        public async Task SetObjectAsync<T>(string key, T objectToCache, TimeSpan expirationRelativeToNow)
        {
            await SetAsyncInRedis(key, objectToCache, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationRelativeToNow });
        }

        public async Task SetObjectAsync<T>(string key, T objectToCache)
        {
            await SetAsyncInRedis(key, objectToCache, null);
        }

        public async Task SetStringAsync(string key, string value)
        {
            await SetStringAsyncInRedis(key, value, null);
        }

        public async Task SetStringAsync(string key, string value, DistributedCacheEntryOptions cacheEntryOptions)
        {
            await SetStringAsyncInRedis(key, value, cacheEntryOptions);
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (TryGetString(key, out string serializedObject))
            {
                try
                {
                    value = JsonSerializer.Deserialize<T>(serializedObject);
                    return true;
                }
                catch (JsonException ex)
                {
                   //logging
                    Remove(key); //If it's invalid json we don't want to keep that value in the cache. 
                }
            }
            value = default;
            return false;
        }

        private void Remove(string key)
        {
            try
            {
                _distributedCache.Remove(key);
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException || ex is RedisServerException || ex is RedisException)
            {
                //logging
            }
        }

        public bool TryGetString(string key, out string value)
        {
            try
            {
                value = _distributedCache.GetString(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException || ex is RedisServerException || ex is RedisException)
            {
                //logging
                //
            }
            value = default;
            return false;
        }

        private Task SetAsyncInRedis<T>(string key, T objectToCache, DistributedCacheEntryOptions options)
        {
            string serializedObject = JsonSerializer.Serialize(objectToCache);
            return SetStringAsyncInRedis(key, serializedObject, options);

        }

        private async Task SetStringAsyncInRedis(string key, string value, DistributedCacheEntryOptions options)
        {
            try
            {
                if (options == null)
                {
                    await _distributedCache.SetStringAsync(key, value);
                }
                else
                {
                    await _distributedCache.SetStringAsync(key, value, options);
                }
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException || ex is RedisServerException || ex is RedisException)
            {
                //logging
            }

        }

        public Task SetStringAsync(string key, string value, TimeSpan expirationRelativeToNow)
        {
            throw new NotImplementedException();
        }
    }
}
