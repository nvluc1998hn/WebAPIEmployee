using EventBusRabbitMQ.Helper;
using EventBusRabbitMQ.Models;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace EventBusRabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        private const string SectionName = "RabbitMq";

        private const string SectionMessageName = "RabbitMqMessage";

        public static IServiceCollection AddBusRabbitMq(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();

                var options = new RabbitMqOptions();
                config.Bind(SectionName, options);
                RabbitMqConnection.CreateRabbitMqConnection(options);
                return services;
            }
        }

        public static IServiceCollection AddSubscriberMessageRabbitMq(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();

                var options = new SubRabbitMqOptions();
                config.Bind(SectionMessageName, options);
                services.AddSingleton(options);
                return services;
            }
        }
    }
}
