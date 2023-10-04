using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Common.Cache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheService cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        public static T Get<T>(this ICacheService cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.HasCache(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        public static byte[] Serialize(object item,string a)
        {
            throw new NotImplementedException();
        }

        public static void RemoveByPattern(this ICacheService cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }

        public static KeyValuePair<string, T>[] GetByPattern<T>(this ICacheService cacheManager, string pattern, IEnumerable<string> keys)
        {
            if (string.IsNullOrEmpty(pattern)) return new KeyValuePair<string, T>[] { };
            if (!pattern.StartsWith("^"))
            {
                pattern = $"^{pattern}";
            }
            if (!pattern.EndsWith("$"))
            {
                pattern = $"{pattern}$";
            }

            pattern = pattern.Replace("*", @"\S+");

            List<KeyValuePair<string, T>> result = new List<KeyValuePair<string, T>>();
            var regex = new Regex(@$"{pattern}", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
            {
                result.Add(new KeyValuePair<string, T>(key, cacheManager.Get<T>(key)));
            }
            return result.ToArray();
        }
    }
}
