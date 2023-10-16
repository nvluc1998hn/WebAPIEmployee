using Base.Common.Cache.Redis.Interface;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache.Redis
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private static ConnectionMultiplexer _connection;
        private readonly object _lock = new();
        private readonly IOptions<RedisCacheOptions> _redisOptions;

        public RedisConnectionWrapper(
            IOptions<RedisCacheOptions> redisOptions)
        {
            _redisOptions = redisOptions;
            _redisOptions.Value.ConfigurationOptions.SyncTimeout = 60000;
        }

        #region Utilities

        protected string GetConnectionString()
        {
            return GetConnection().Configuration;
        }

        /// <summary>
        /// Get connection to Redis servers
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                //Connection disconnected. Disposing connection...
                _connection?.Dispose();

                //Creating new instance of Redis Connection
                if (_redisOptions.Value.Configuration != null)
                {
                    _connection = ConnectionMultiplexer.Connect(_redisOptions.Value.Configuration);
                }
                else
                {
                    _connection = ConnectionMultiplexer.Connect(_redisOptions.Value.ConfigurationOptions);
                }
            }

            return _connection;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Obtain an interactive connection to a database inside redis
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value</param>
        /// <returns>Redis cache database</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            var defaultDb = _redisOptions.Value.ConfigurationOptions.DefaultDatabase.HasValue ? _redisOptions.Value.ConfigurationOptions.DefaultDatabase.Value : -1;
            return GetConnection().GetDatabase(db ?? defaultDb);
        }

        /// <summary>
        /// Obtain a configuration API for an individual server
        /// </summary>
        /// <param name="endPoint">The network endpoint</param>
        /// <returns>Redis server</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// Gets all endpoints defined on the server
        /// </summary>
        /// <returns>Array of endpoints</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Delete all the keys of the database
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value<</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        /// <summary>
        /// Perform some action with Redis distributed lock
        /// </summary>
        /// <param name="resource">The thing we are locking on</param>
        /// <param name="expirationTime">The time after which the lock will automatically be expired by Redis</param>
        /// <param name="action">Action to be performed with locking</param>
        /// <returns>True if lock was acquired and action was performed; otherwise false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            return false;
        }

        /// <summary>
        /// Release all resources associated with this object
        /// </summary>
        public void Dispose()
        {
            //dispose ConnectionMultiplexer
            if (_connection != null)
                _connection.Dispose();
        }

        #endregion Methods
    }
}
