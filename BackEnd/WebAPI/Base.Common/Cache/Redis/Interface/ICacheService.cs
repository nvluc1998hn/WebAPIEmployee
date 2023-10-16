using Base.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache.Redis.Interface
{
    public interface IServiceCache : ICacheService
    {
        /// <summary> Lấy toàn bộ dữ liệu của sorted set </summary>
        List<T> SortedGet<T>(string key);

        /// <summary> Kiểm tra thời gian còn lại của cache </summary>
        TimeSpan? KeyTimeToLive(string key);
    }
}
