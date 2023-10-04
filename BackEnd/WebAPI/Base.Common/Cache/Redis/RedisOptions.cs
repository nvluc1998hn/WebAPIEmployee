using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache.Redis
{
    public class RedisOptions
    {
        /// <summary>
        /// is enabke
        /// </summary>
        public bool Enabled { get; set; }

        public string ServiceName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConnectRetry { get; set; }
        public bool AllowAdmin { get; set; }
        public bool ResolveDns { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public int DefaultDatabase { get; set; }
        public int ConnectTimeout { get; set; }
    }
}
