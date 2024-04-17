﻿using Base.Common.Dapper;
using Base.Common.DBContext;
using Base.Common.Implementations;
using Base.Common.Interfaces;
using EmployeeManagement.Database;
using EmployeeManagement.Database.Repositories.Implementations;
using EmployeeManagement.Database.Repositories.Interfaces;
using EmployeeManagement.EfCore.Services.Implementations;
using EmployeeManagement.EfCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeManagement.EfCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<ICustomerService>()
                 .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddHttpClient();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

    }
}
