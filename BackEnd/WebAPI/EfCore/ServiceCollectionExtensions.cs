using EmployeeManagement.Database.Context.DbOptions;
using EmployeeManagement.Database.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.EfCore
{
    public static class ServiceCollectionExtensions
    {
        private const string SectionName = "Infrastructures:SqlServer";

        public static IServiceCollection AddEfCoreSqlServer<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            //services.AddServiceByIntefaceInAssembly<DbOptions>(typeof(IRepositoryAsync<>));
            //services.AddServiceByIntefaceInAssembly<DbOptions>(typeof(IQueryRepository<>));

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
             //       services.AddScoped<IUnitOfWork, UnitOfWork>();
                }
                else
                {
               //     services.AddScoped<IUnitOfWork, EfUnitOfWork>();
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
            else
            {
                //services.AddScoped<IUnitOfWork, EfUnitOfWork>();
                //services.AddDbContext<ReportServer1Context>(o => o.UseSqlServer(options.ConnString1));
                //services.AddDbContext<ReportServer2Context>(o => o.UseSqlServer(options.ConnString2));
                //services.AddDbContext<ReportServer3Context>(o => o.UseSqlServer(options.ConnString3));
                //services.AddDbContext<ReportServer4Context>(o => o.UseSqlServer(options.ConnString4));
                //services.AddDbContext<ReportServer5Context>(o => o.UseSqlServer(options.ConnString5));
                //services.AddDbContext<ReportServer6Context>(o => o.UseSqlServer(options.ConnString6));
                //services.AddDbContext<ReportServer100Context>(o => o.UseSqlServer(options.ConnString100));
                //services.AddDbContext<TDbContext>((sp, o) =>
                //{
                //    o.UseInMemoryDatabase("DefaultMainDb");
                //});
            }

            services.AddScoped<DbContext>(resolver => resolver.GetService<TDbContext>());

            return services;
        }
    }
}
