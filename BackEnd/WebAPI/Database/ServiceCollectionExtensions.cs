﻿using EmployeeManagement.Database.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeManagement.Database
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<ICustomerRepository>()
                 .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            return services;
        }
    }
}
