using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.Helper
{
    public interface IRedisDestributeCache
    {
        /// <summary>
        /// Attempts to fetch a cached value of type T identified by key. Returns false if key is not available or fetch fails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGet<T>(string key, out T value);
        /// <summary>
        /// Caches the value of type T with a key for a specified time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="objectToCache"></param>
        /// <param name="expirationRelativeToNow"></param>
        /// <returns></returns>
        Task SetObjectAsync<T>(string key, T objectToCache, TimeSpan expirationRelativeToNow);
        /// <summary>
        /// Caches the value of type T with a key without expiry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="objectToCache"></param>
        /// <returns></returns>
        Task SetObjectAsync<T>(string key, T objectToCache);
        /// <summary>
        /// Attempts to fetch a cached string identified by key. Returns false if key is not available or fetch fails
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetString(string key, out string value);
        /// <summary>
        /// Caches the string value with a key for a specified time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetStringAsync(string key, string value);
        /// <summary>
        /// Caches the string value with a key without expiry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationRelativeToNow"></param>
        /// <returns></returns>
        Task SetStringAsync(string key, string value, TimeSpan expirationRelativeToNow);
        /// <summary>
        /// Removes the cached value identified by the specified key, if it exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
