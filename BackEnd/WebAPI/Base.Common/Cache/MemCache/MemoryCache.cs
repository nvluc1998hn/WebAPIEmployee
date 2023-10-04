using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace Base.Common.Cache.MemCache
{
    public class MemoryCache : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public int DefaultCacheTime { get; set; } = 15;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _memoryCache.Get<T>(key.ToUpper());
            }
            else
            {
                return default;
            }
        }

        public T[] Get<T>(string[] keys)
        {
            if (keys == null || keys.Length == 0) return default;
            List<T> results = new();
            foreach (var key in keys)
            {
                var res = _memoryCache.Get<T>(key.ToUpper());
                if (res != null)
                {
                    results.Add(res);
                }
                else
                {
                    results.Add(default);
                }
            }
            return results?.ToArray();
        }

        public KeyValuePair<string, T>[] GetByPattern<T>(string pattern, int offset = 5000)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            object entries = this._memoryCache.GetType().GetField("_entries", flags).GetValue(this._memoryCache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string>();
            foreach (var cacheItem in cacheItems.Keys)
            {
                keys.Add(cacheItem.ToString());
            }
            return this.GetByPattern<T>(pattern, keys);
        }

        public bool HasCache(string key)
        {
            throw new NotImplementedException();
        }

        public T Hget<T>(string key, string field)
        {
            throw new NotImplementedException();
        }

        public void Hset(string key, string field, object data, int cacheTime)
        {
            throw new NotImplementedException();
        }

        public void Hset(string key, HashEntry[] data, int cacheTime)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPattern(string pattern, int offset = 5000)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            object entries = this._memoryCache.GetType().GetField("_entries", flags).GetValue(this._memoryCache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string>();
            foreach (var cacheItem in cacheItems.Keys)
            {
                keys.Add(cacheItem.ToString());
            }
            this.RemoveByPattern(pattern, keys);
        }

        public byte[] Serialize(object item)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object data, int? cacheTime = null)
        {
            if (data == null)
                return;
            if (!cacheTime.HasValue)
            {
                cacheTime = DefaultCacheTime;
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime.Value));
            _memoryCache.Set(key.ToUpper(), data, cacheOptions);
        }

        public void Set(KeyValuePair<string, object>[] values)
        {
            if (values == null || values.Length == 0)
                return;
            foreach(var value in values)
            {

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(int.MaxValue));
                _memoryCache.Set(value.Key.ToUpper(), value.Value, cacheOptions);
            }
        }

        public void SetToCache(List<KeyValuePair<string, object>> tempList, int offset)
        {
            throw new NotImplementedException();
        }

        public List<T> SortedGet<T>(string key, int start, int stop, Order order = Order.Ascending)
        {
            throw new NotImplementedException();
        }

        public void SortedSet<T>(string key, List<T> data, int? cacheTime = null)
        {
            throw new NotImplementedException();
        }

        public long SortedSetLength(string redisKey)
        {
            throw new NotImplementedException();
        }
    }
}
