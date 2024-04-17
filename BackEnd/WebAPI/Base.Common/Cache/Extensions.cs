using Base.Common.Cache.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Net;
using Base.Common.Cache.MemCache;
using Base.Common.Cache.Redis.Interface;
using Base.Common.Dapper;
using Base.Common.DBContext;
using Base.Common.Implementations;
using Base.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        private const string SectionNameEF = "Infrastructures:SqlServer";

        public static IServiceCollection AddEfCoreSqlServer<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            // Chỗ này dki dapper
            services.AddServiceByIntefaceInAssembly<DbOptions>(typeof(IRepositoryAsync<,>));

            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();

            var options = new DbOptions();
            config.Bind(SectionNameEF, options);

            services.AddSingleton(options);

            if (options.Enabled)
            {
                services.AddTransient<ISqlConnectionFactory, SqlConnectionFactory>();

                //Đăng kí sử dụng DapperExtensions
                DapperExtensions.Initialize(options);

                //có sử dụng dapper hay ko. EnabledDapper=true thì dùng Dapper ngược lại dùng EF
                if (options.EnabledDapper)
                {
                    services.AddScoped(typeof(IRepositoryAsync<,>), typeof(GenericRepository<,>));

                }
                else
                {
                   // Bỏ dùng linq
                  //  services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
                }


                services.AddDbContext<TDbContext>((sp, o) =>
                {
                    string connectionString = options.ConnString;
                    o.UseSqlServer(connectionString,
                            sqlOptions =>
                            {
                                sqlOptions.EnableRetryOnFailure(
                                    15,
                                    TimeSpan.FromSeconds(30),
                                    null);
                            })
                        .EnableSensitiveDataLogging();
                });
            }
            services.AddScoped<DbContext>(resolver => resolver.GetService<TDbContext>());
            return services;
        }


        public static IServiceCollection AddServiceByIntefaceInAssembly<TRegisteredAssemblyType>(this IServiceCollection services, Type interfaceType)
        {
            services.Scan(s =>
                s.FromAssemblyOf<TRegisteredAssemblyType>()
                    .AddClasses(c => c.AssignableTo(interfaceType))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
