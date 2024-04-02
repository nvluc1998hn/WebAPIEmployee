using RawRabbit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Models
{
    /// <summary>
    /// Option cấu hình rabbit mq
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 05/03/2024 created
    /// </Modified>
    public class RabbitMqBaseOptions : RawRabbitConfiguration
    {
        public bool Enabled { get; set; }

        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }
    }

    public class RabbitMqOptions : RabbitMqBaseOptions
    {
        public string Namespace { get; set; }
        public int Retries { get; set; }
        public int RetryInterval { get; set; }
    }

    public class SubRabbitMqOptions : RawRabbitConfiguration
    {
        public string Namespace { get; set; }

        public int Retries { get; set; }

        public int RetryInterval { get; set; }

        public bool Enabled { get; set; }

        public bool UseProtobuf { get; set; }

        public string QueueName { get; set; }

        public string RoutingKey { get; set; }

        public string ExchangeName { get; set; }
    }


}
