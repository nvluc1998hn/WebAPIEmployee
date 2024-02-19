using Base.Mongo.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MongoDB.Driver;

namespace Base.Mongo
{
    public static class Extensions
    {
        private const string SectionName = "Infrastructures:Mongo";
      
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var options = new MongoDbOptions();

                config.Bind(SectionName, options);

                services.AddSingleton(options);

                // Log thông tin option
                Log.Logger.Warning($"Extensions.AddMongoDb(), MongoDbOptions: {JsonConvert.SerializeObject(options)}");

                if (!options.Enabled) return services;

                services.AddSingleton(sp =>
                {
                    var client = new MongoClient(options.ConnectionString);

                    return client.GetDatabase(options.Database);
                });
                services.AddScoped(typeof(IMongoBaseRepository<,>), typeof(MongoBaseRepository<,>));
                BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
                return services;
            }
        }
    }
}
