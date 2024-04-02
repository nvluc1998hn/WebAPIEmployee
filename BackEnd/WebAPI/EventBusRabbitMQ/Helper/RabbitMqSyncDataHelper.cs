using EventBusRabbitMQ.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using RawRabbit;
using Newtonsoft.Json.Linq;
using Serilog;
using Newtonsoft.Json;

namespace EventBusRabbitMQ.Helper
{
    public static class RabbitMqSyncDataHelper
    {
        private static readonly Guid InstanceId = Guid.NewGuid();

        public static void SubscribeSyncInstance<TEntity>(Action<List<TEntity>> handler, bool isDeleteEvent = false)
        {
            try
            {
                //Kiểm tra xem có phải là bảng không, nếu có thì mới đăng ký nghe rabbitmq
                var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
                if (tableAttribute != null)
                {
                    RabbitMqBaseOptions options;
                    IBusClient busClient;

                    options = RabbitMqConnection.RabbitV3Options;
                    busClient = RabbitMqConnection.RabbitV3BusClient;

                    if (options.Enabled && busClient != null)
                    {
                        //Lấy các thông tin routingKey, queue
                        var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                        var routingKey = $"{options.RoutingKey}.{tableAttribute.Name}";
                        if (isDeleteEvent)
                        {
                            routingKey += "_DELETE";
                        }

                        var queueName = $"{routingKey}_{ip}";

                        // Lắng nghe rabbitmq
                        busClient.SubscribeAsync<SyncEvent>(_event =>
                        {
                            try
                            {
                                if (_event.Data?.Count > 0)
                                {
                                    var props = typeof(TEntity).GetProperties().Where(c => Attribute.IsDefined(c, typeof(ColumnAttribute))).ToList();

                                    // Parse từng dòng dữ liệu
                                    var entities = new List<TEntity>();
                                    foreach (var item in _event.Data)
                                    {
                                        var entity = item.ToObject<TEntity>();
                                        foreach (var prop in props)
                                        {
                                            if (item.TryGetValue(prop.GetCustomAttribute<ColumnAttribute>().Name, StringComparison.OrdinalIgnoreCase, out var value))
                                            {
                                                prop.SetValue(entity, value.Type == JTokenType.Null ? null : Convert.ChangeType(value, prop.PropertyType));
                                            }
                                        }
                                        entities.Add(entity);
                                    }
                                    handler(entities);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log thông tin ra màn hình
                                Log.Logger.Fatal($"RabbitMqSyncDataHelper.SubscribeSyncInstance() _event: {JsonConvert.SerializeObject(_event)}", ex);
                            }

                            return Task.CompletedTask;
                        },
                                   ctx => ctx.UseSubscribeConfiguration(
                                       cfg => cfg.Consume(
                                           c => c.WithRoutingKey(routingKey))
                                                 .FromDeclaredQueue(
                                           q => q.WithName(queueName))
                                                 .OnDeclaredExchange(
                                           e => e.WithName(options.ExchangeName))
                                   ));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"RabbitMqSyncDataHelper.SubscribeSyncInstance() có lỗi: {ex.Message}", ex);
            }
        }
    }
}
