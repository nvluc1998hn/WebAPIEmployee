using Base.Common.Dapper;
using Base.Common.DBContext;
using Base.Common.Implementations;
using Base.Common.Interfaces;
using EmployeeManagement.Database;
using EmployeeManagement.Database.Repositories.Implementations;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Implementations;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeManagement.EfCore
{
    public static class ServiceCollectionExtensions
    {
        private const string SectionName = "Infrastructures:SqlServer";

        public static IServiceCollection AddEfCoreSqlServer<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            // Chỗ này dki dapper
            services.AddServiceByIntefaceInAssembly<DbOptions>(typeof(IRepositoryAsync<,>));

            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();

            var options = new DbOptions();
            config.Bind(SectionName, options);

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
                    services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
                }

                // Service
                services.Scan(scan => scan
                 .FromAssemblyOf<IEmployeeService>()
                      .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                         .AsImplementedInterfaces()
                         .WithScopedLifetime());


                // Repository
                services.Scan(scan => scan
                 .FromAssemblyOf<IEmployeeRepository>()
                      .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                         .AsImplementedInterfaces()
                         .WithScopedLifetime());


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
            else
            {
            }

            services.AddScoped<DbContext>(resolver => resolver.GetService<TDbContext>());

            return services;
        }
    }
}
