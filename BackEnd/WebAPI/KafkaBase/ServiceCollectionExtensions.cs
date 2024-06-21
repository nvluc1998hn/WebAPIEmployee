using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaBase
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafKa(this IServiceCollection services)
        {
            services.AddHostedService<KafKaProducerHostedClientService>();

            services.AddHostedService<KafKaProducerHostedService>();

            return services;
        }

    }
}
