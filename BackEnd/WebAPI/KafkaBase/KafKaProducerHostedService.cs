using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaBase
{
    public class KafKaProducerHostedService : IHostedService
    {
        private readonly ILogger<KafKaProducerHostedService> _logger;   
        private IProducer<string, string> _producer;

        public KafKaProducerHostedService(ILogger<KafKaProducerHostedService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };
            _producer = new ProducerBuilder<string, string>(config).Build(); 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
          for(var i = 1; i < 10; i++)
            {
                await _producer.ProduceAsync("demo-kafka", new Message<string, string>()
                {
                    Value = "123124",
                    Key ="atm"
                }, cancellationToken);

            }
            await _producer.ProduceAsync("demo-kafka", new Message<string, string>()
            {
                Value = "1231245",
                Key ="acm"
            }, cancellationToken);

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
