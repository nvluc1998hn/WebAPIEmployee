using Admin.Application.Interfaces;
using Admin.Application.Mapper;
using Admin.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapperSetup();

            services.Scan(scan => scan
            .FromAssemblyOf<IAdminUserService>()
                 .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.AddSingleton<IAuthenticationServiceSingleton, AuthenticationServiceSingleton>();

            services.AddHttpClient();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
