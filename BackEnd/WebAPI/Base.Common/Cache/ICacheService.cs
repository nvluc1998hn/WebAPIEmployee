using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache
{
    /// <summary> cache manage </summary>
    public interface ICacheService : IDisposable
    {
        int DefaultCacheTime { get; set; }

        /// <summary> Gets or sets the value associated with the spectified key; </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key"> </param>
        /// <returns> </returns>
        T Get<T>(string key);

        List<T> SortedGet<T>(string key, int start, int stop, Order order = Order.Ascending);

        /// <summary> Get List data from array of keys </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="key"> </param>
        /// <returns> </returns>
        T[] Get<T>(string[] key);

        /// <summary> Get list value by pattern </summary>
        /// <param name="pattern"> </param>
        /// <returns> </returns>
        KeyValuePair<string, T>[] GetByPattern<T>(string pattern, int offset = 5000);

        /// <summary> Adds the specified key and object to the cache. </summary>
        /// <param name="key"> key </param>
        /// <param name="data"> Data </param>
        /// <param name="cacheTime"> Cache time </param>
        void Set(string key, object data, int? cacheTime = null);

        void SortedSet<T>(string key, List<T> data, int? cacheTime = null);

        long SortedSetLength(string redisKey);

        /// <summary> Set cache theo Key Value </summary>
        /// <param name="values"> </param>
        void Set(KeyValuePair<string, object>[] values);

        /// <summary> Set to cache with offset </summary>
        /// <param name="tempList"> </param>
        /// <param name="offset"> </param>
        void SetToCache(List<KeyValuePair<string, object>> tempList, int offset);

        /// <summary> Kiểm tra tồn tại cache </summary>
        bool HasCache(string key);

        /// <summary> Removes the value with the specified key from the cache </summary>
        /// <param name="key"> /key </param>
        void Remove(string key);

        /// <summary> Removes items by pattern </summary>
        /// <param name="pattern"> pattern </param>
        void RemoveByPattern(string pattern, int offset = 5000);

        /// <summary> Clear all cache data </summary>
        void Clear();

        T Hget<T>(string key, string field);

        void Hset(string key, string field, object data, int cacheTime);

        void Hset(string key, HashEntry[] data, int cacheTime);

        /// <summary> Serialize object to save cache </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        byte[] Serialize(object item);

        /// <summary> Deserialize data from cache to object </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="serializedObject"> </param>
        /// <returns> </returns>
        T Deserialize<T>(byte[] serializedObject);
    }
}
