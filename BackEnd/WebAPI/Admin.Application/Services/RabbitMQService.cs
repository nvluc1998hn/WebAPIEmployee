using Microsoft.EntityFrameworkCore.Metadata;
using MongoDB.Driver.Core.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using IConnection = RabbitMQ.Client.IConnection;
using IModel = RabbitMQ.Client.IModel;
using RabbitMQ.Client.Events;
using Admin.Application.Interfaces;
using System.Threading.Channels;
using MongoDB.Driver.Core.Bindings;


namespace Admin.Application.Services
{
    public class RabbitMQService: IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // RabbitMQ server address
                Port = 5672,            // Default port
                UserName = "guest",     // Default username
                Password = "guest"      // Default password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }

        public void SendMessage(string queueName, string message)
        {
            string deadLetterExchange = "abcd";

            // Thiết lập các thuộc tính cho hàng đợi với x-dead-letter-exchange
            var arguments = new Dictionary<string, object>{
            { "x-dead-letter-exchange", deadLetterExchange }};

            _channel.QueueDelete(queue: queueName);

            _channel.ExchangeDeclare(exchange: deadLetterExchange, type: ExchangeType.Fanout, durable: true);

            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: arguments);
         
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void ReceiveMessage(string queueName, Action<string> messageHandler)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                messageHandler.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

    }
}
