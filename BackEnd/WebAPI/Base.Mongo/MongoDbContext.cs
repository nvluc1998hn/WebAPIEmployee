﻿using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;

namespace Base.Mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbOptions _options;

        public MongoDbContext(MongoDbOptions options)
        {
            _options = options;
            var client = new MongoClient(options.ConnectionString);
            _database = client.GetDatabase(options.Database);

            // Log thông tin option
            Log.Logger.Warning($"MongoDbContext.Contructor(), MongoDbOptions: {JsonConvert.SerializeObject(_options)}");
        }

        public IMongoCollection<TEntity> Collection<TEntity>()
            where TEntity : class
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).FullName);
        }
    }
}