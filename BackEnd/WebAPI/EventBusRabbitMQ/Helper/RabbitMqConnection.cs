using EventBusRabbitMQ.Models;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Helper
{
    public static class RabbitMqConnection
    {
        private static RabbitMqOptions _rabbitV3Options = null;

        public static RabbitMqOptions RabbitV3Options
        {
            get
            {
                return _rabbitV3Options;
            }
        }
        private static IBusClient _rabbitV3BusClient = null;

        public static IBusClient RabbitV3BusClient
        {
            get
            {
                return _rabbitV3BusClient;
            }
        }

        private static RabbitMqSyncCommonOptions _rabbitCommonOptions = null;

        public static RabbitMqSyncCommonOptions RabbitCommonOptions
        {
            get
            {
                return _rabbitCommonOptions;
            }
        }
        private static IBusClient _rabbitCommonBusClient = null;

        public static IBusClient RabbitCommonBusClient
        {
            get
            {
                return _rabbitCommonBusClient;
            }
        }

        public static void CreateRabbitMqConnection<TOption>(TOption options) where TOption : RawRabbitConfiguration
        {
            try
            {
                var type = typeof(TOption);
                if (type == typeof(RabbitMqOptions))
                {
                    _rabbitV3Options = options as RabbitMqOptions;
                    if (_rabbitV3Options.Enabled && _rabbitV3BusClient == null)
                    {
                        _rabbitV3BusClient = RawRabbitFactory.CreateSingleton(RawRabbitHelper.GetConfigRawRabbit(_rabbitV3Options));
                    }

                    // Log thông tin option
                    Log.Logger.Warning($"RabbitMqConnection.CreateRabbitMqConnection(), _rabbitV3Options: {JsonConvert.SerializeObject(_rabbitV3Options)}");

                }
                else if (type == typeof(RabbitMqSyncCommonOptions))
                {
                    _rabbitCommonOptions = options as RabbitMqSyncCommonOptions;
                 
                    if (_rabbitCommonOptions.Enabled && _rabbitCommonBusClient == null)
                    {
                        _rabbitCommonBusClient = RawRabbitFactory.CreateSingleton(RawRabbitHelper.GetConfigRawRabbit(_rabbitCommonOptions));
                    }

                    // Log thông tin option
                    Log.Logger.Warning($"RabbitMqConnection.CreateRabbitMqConnection(), _rabbitCommonOptions: {JsonConvert.SerializeObject(_rabbitCommonOptions)}");
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"RabbitMqConnection.CreateRabbitMqConnection() có lỗi: {ex.Message}", ex);
            }
        }
    }
}
