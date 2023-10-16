using Base.Common.Cache.Redis.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache.Redis
{
    public class RedisCacheService : IServiceCache
    {
        #region Fields

        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly ILogger<RedisCacheService> _logger;

        public int DefaultCacheTime { get; set; } = 1440;

        #endregion Fields

        #region Constructor

        public RedisCacheService(IRedisConnectionWrapper connectionWrapper, ILogger<RedisCacheService> logger)
        {
            _connectionWrapper = connectionWrapper ?? throw new ArgumentNullException(nameof(connectionWrapper));
            _logger = logger;
        }

        #endregion Constructor

        public virtual bool HasCache(string key)
        {
            var hasCache = false;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    hasCache = _connectionWrapper.GetDatabase().KeyExists(key.ToUpper());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }

            return hasCache;
        }

        public virtual TimeSpan? KeyTimeToLive(string key)
        {
            return _connectionWrapper.GetDatabase()?.KeyTimeToLive(key);
        }

        public virtual T Get<T>(string key)
        {
            T data = default;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var valueStr = _connectionWrapper.GetDatabase().StringGet(key.ToUpper());
                    if (!string.IsNullOrEmpty(valueStr))
                    {
                        data = Deserialize<T>(valueStr);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }

            return data;
        }

        public T[] Get<T>(string[] keys)
        {
            var data = Array.Empty<T>();

            try
            {
                if (keys?.Length > 0)
                {
                    var redisKeys = keys.Select(m => (RedisKey)m);
                    List<RedisValue> rValues = new();
                    RedisValue[] arValue = Array.Empty<RedisValue>();

                    var offset = 1000;
                    var total = redisKeys.Count();
                    int count = total / offset;

                    var db = _connectionWrapper.GetDatabase();

                    for (int i = 0; i <= count; i++)
                    {
                        var part = redisKeys.Skip(offset * i).Take(offset)?.ToArray();
                        if (part != null)
                        {
                            rValues.AddRange(db.StringGet(part).ToList());
                        }
                    }

                    List<T> listReturn = new();
                    rValues.ForEach(rVal =>
                    {
                        listReturn.Add(Deserialize<T>(rVal));
                    });

                    data = listReturn.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }

            return data;
        }

        public virtual void Set(KeyValuePair<string, object>[] values)
        {
            try
            {
                if (values?.Length > 0)
                {
                    List<KeyValuePair<RedisKey, RedisValue>> tempList = new();

                    foreach(var item in values)
                    {
                        var jsonString = JsonConvert.SerializeObject(item.Value);
                        var dataToCache = Encoding.UTF8.GetBytes(jsonString);
                        tempList.Add(new KeyValuePair<RedisKey, RedisValue>(item.Key, dataToCache));
                    }
                    _connectionWrapper.GetDatabase().StringSet(tempList.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }
        }

        public virtual void Set(string key, object data, int? cacheTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && data != null)
                {
                    if (!cacheTime.HasValue)
                    {
                        cacheTime = DefaultCacheTime;
                    }

                    var entryBytes = Serialize(data);
                    var expiresIn = TimeSpan.FromMinutes(cacheTime.Value);

                    _connectionWrapper.GetDatabase().StringSet(key.ToUpper(), entryBytes, expiresIn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }
        }

        public virtual void Remove(string key)
        {
            _connectionWrapper.GetDatabase()?.KeyDelete(key);
        }

        public virtual void RemoveByPattern(string pattern, int offset = 5000)
        {
            try
            {
                var _db = _connectionWrapper.GetDatabase();
                var command = _db.Execute("keys", pattern);
                if (command != null)
                {
                    var keys = (RedisKey[])command;
                    var total = keys.Length;
                    int count = total / offset;
                    for (int i = 0; i <= count; i++)
                    {
                        var part = keys.Skip(offset * i).Take(offset)?.ToArray();
                        _db.KeyDelete(part);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }
        }

        public virtual void Clear()
        {
            try
            {
                foreach (var ep in _connectionWrapper.GetEndPoints())
                {
                    var server = _connectionWrapper.GetServer(ep);
                    var keys = server.Keys(database: _connectionWrapper.GetDatabase().Database);
                    foreach (var key in keys)
                    {
                        Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }
        }

        public virtual void Dispose()
        {
            _connectionWrapper?.Dispose();
        }


        #region SortedSet

        public virtual List<T> SortedGet<T>(string key, int start, int stop, Order order = Order.Ascending)
        {
            List<T> data = null;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var redisData = _connectionWrapper.GetDatabase().SortedSetRangeByRank(key.ToUpper(), start, stop, order);
                    data = redisData?.Select(item => JsonConvert.DeserializeObject<T>(item)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }

            return data;
        }

        public virtual List<T> SortedGet<T>(string key)
        {
            List<T> data = null;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var redisData = _connectionWrapper.GetDatabase().SortedSetScan(key.ToUpper());
                    data = redisData?.Select(item => JsonConvert.DeserializeObject<T>(item.Element)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }

            return data;
        }

        public virtual void SortedSet<T>(string key, List<T> data, int? cacheTime = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && data != null)
                {
                    if (!cacheTime.HasValue)
                    {
                        cacheTime = DefaultCacheTime;
                    }

                    List<SortedSetEntry> values = new();

                    for (int i = 0; i < data.Count; i++)
                    {
                        string member = JsonConvert.SerializeObject(data[i]);
                        values.Add(new SortedSetEntry(member, i + 1));
                    }

                    var db = _connectionWrapper.GetDatabase();
                    var keyUp = key.ToUpper();
                    db.SortedSetAdd(keyUp, values.ToArray());
                    db.KeyExpire(keyUp, TimeSpan.FromMinutes(cacheTime.Value));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi RedisCacheService - {MethodBase.GetCurrentMethod().Name}: {ex}");
            }
        }

        public virtual long SortedSetLength(string key)
        {
            return _connectionWrapper.GetDatabase()?.SortedSetLength(key.ToUpper()) ?? 0;
        }

        #endregion SortedSet

        #region Utilities

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion Utilities

        public virtual T Hget<T>(string key, string field)
        {
            var rValue = _connectionWrapper.GetDatabase().HashGet(key.ToUpper(), field);
            if (!rValue.HasValue)
                return default(T);
            var result = Deserialize<T>(rValue);

            return result;
        }

        public void Hset(string key, string field, object data, int cacheTime)
        {
            if (data == null)
                return;

            var entryBytes = Serialize(data);
            _connectionWrapper.GetDatabase().HashSet(key.ToUpper(), field, entryBytes);
            if (cacheTime > 0)
            {
                var expiresIn = TimeSpan.FromMinutes(cacheTime);
                _connectionWrapper.GetDatabase().KeyExpire(key.ToUpper(), expiresIn);
            }
        }

        public void SetToCache(List<KeyValuePair<string, object>> tempList, int offset)
        {
            var total = tempList.Count;
            int count = total / offset;
            for (int i = 0; i <= count; i++)
            {
                var part = tempList.Skip(offset * i).Take(offset)?.ToArray();
                if (part != null)
                {
                    Set(part);
                }
            }
        }

        public KeyValuePair<string, T>[] GetByPattern<T>(string pattern, int offset = 5000)
        {
            var _db = _connectionWrapper.GetDatabase();
            List<KeyValuePair<string, T>> result = new List<KeyValuePair<string, T>>();
            var command = _db.Execute("keys", pattern);
            if (command != null)
            {
                var keys = (RedisKey[])command;
                var total = keys.Length;
                int count = total / offset;
                for (int i = 0; i <= count; i++)
                {
                    var part = keys.Skip(offset * i).Take(offset)?.ToArray();
                    if (part != null)
                    {
                        try
                        {
                            var vals = _db.StringGet(part);
                            for (int j = 0; j < vals.Length; j++)
                            {
                                if (vals[j].HasValue)
                                {
                                    result.Add(new KeyValuePair<string, T>(part[j], Deserialize<T>(vals[j])));
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            return result.ToArray();
        }

        public void Hset(string key, HashEntry[] data, int cacheTime)
        {
            if (data == null || !data.Any())
                return;
            try
            {
                _connectionWrapper.GetDatabase().HashSet(key.ToUpper(), data);
                if (cacheTime > 0)
                {
                    var expiresIn = TimeSpan.FromMinutes(cacheTime);
                    _connectionWrapper.GetDatabase().KeyExpire(key.ToUpper(), expiresIn);
                }
            }
            catch (Exception) { return; }
        }

        byte[] ICacheService.Serialize(object item)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(item);
                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception)
            {
                return new byte[] { };
            }
        }

        T ICacheService.Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
