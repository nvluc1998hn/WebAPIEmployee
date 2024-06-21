using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafka.Public;
using Kafka.Public.Loggers;

namespace KafkaBase
{
    public class KafKaProducerHostedClientService : IHostedService
    {
        private readonly ILogger<KafKaProducerHostedClientService> _logger;
        private readonly ClusterClient _clusterClient;

        public KafKaProducerHostedClientService(ILogger<KafKaProducerHostedClientService> logger)
        {
            _logger = logger;

            _clusterClient = new ClusterClient(new Configuration
            {
                Seeds = "localhost:9092"
            },new ConsoleLogger());

        }


        public Task StartAsync(CancellationToken cancellationToken)
        {

            _clusterClient.ConsumeFromLatest("demo-kafka",0);
            _clusterClient.MessageReceived += record =>
            {
                    _logger.LogInformation(Encoding.UTF8.GetString(record.Value as byte[]));
            };
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _clusterClient.Dispose();
            return Task.CompletedTask;
        }
    }
}
