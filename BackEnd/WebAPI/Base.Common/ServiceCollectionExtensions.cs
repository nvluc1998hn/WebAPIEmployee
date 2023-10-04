using Base.Common.Cache.MemCache;
using Base.Common.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Đăng ký các dịch vụ dùng chung cho toàn Solution
        /// </summary>
        public static IServiceCollection AddServiceCommon(this IServiceCollection services)
        {
            services.AddRedisServices();
        
            return services;
        }
    }
}
