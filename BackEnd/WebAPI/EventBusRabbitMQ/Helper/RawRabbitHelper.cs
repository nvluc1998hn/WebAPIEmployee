using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using RawRabbit.Enrichers.MessageContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Helper
{
    public static class RawRabbitHelper
    {
        public static RawRabbitOptions GetConfigRawRabbit<TOption>(TOption option) where TOption : RawRabbitConfiguration
        {
            var rawoption = new RawRabbitOptions
            {
                ClientConfiguration = new RawRabbitConfiguration
                {
                    
                    Username = option.Username,
                    Password = option.Password,
                    VirtualHost = "/",
                    Port = option.Port,
                    Hostnames = option.Hostnames,
                    RequestTimeout = option.RequestTimeout,
                    PublishConfirmTimeout = TimeSpan.FromSeconds(1),
                    RecoveryInterval = TimeSpan.FromSeconds(1),
                    PersistentDeliveryMode = true,
                    AutoCloseConnection = true,
                    AutomaticRecovery = true,
                    TopologyRecovery = true,
                    Exchange = option.Exchange,
                    Queue = option.Queue
                },
                DependencyInjection = ioc =>
                {
                    ioc.AddSingleton(option);
                },
                Plugins = p => p
                .UseAttributeRouting()
                .UseRetryLater()
                .UseContextForwarding()
            };
            return rawoption;
        }
    }
}
