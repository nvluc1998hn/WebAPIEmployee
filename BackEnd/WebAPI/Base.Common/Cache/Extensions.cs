using Base.Common.Cache.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Net;
using Base.Common.Cache.MemCache;
using Base.Common.Cache.Redis.Interface;

namespace Base.Common.Cache
{
    public static class Extensions
    {
        private const string SectionName = "Cache:Service:Redis";

        public static IServiceCollection AddRedisServices(this IServiceCollection services)
        {
            var redisOptions = new RedisOptions();
            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            config.Bind(SectionName, redisOptions);
            services.AddSingleton(redisOptions);
            if (redisOptions.Enabled)
            {

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = new ConfigurationOptions
                    {
                        EndPoints =
                        {
                            new DnsEndPoint(redisOptions?.Host??"redis", redisOptions?.Port ?? 6379)
                        },
                        ResolveDns = redisOptions?.ResolveDns ?? true,
                        //AbortOnConnectFail = redisOptions?.AbortOnConnectFail ?? false,
                        //ServiceName = redisOptions?.ServiceName,
                        ConnectRetry = redisOptions?.ConnectRetry ?? 10,
                        AllowAdmin = redisOptions?.AllowAdmin ?? true,
                        DefaultDatabase = redisOptions?.DefaultDatabase ?? -1,
                        //ConnectTimeout = redisOptions?.ConnectTimeout ?? 5
                    };
                });
                services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                services.AddSingleton<ICacheService, RedisCacheService>();
            }
            else
            {
                services.AddSingleton<ICacheService, MemoryCache>();
                services.AddMemoryCache();
            }
            services.AddDistributedMemoryCache();
            return services;
        }
    }
}
