using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Models
{
    public class SyncEvent
    {
        public Guid ID { get; protected set; } = Guid.NewGuid();
        public string RoutingKey { get; set; }
        public long TimeSend { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
        public List<JObject> Data { get; set; }
        public Guid? InstanceIdV3 { get; set; }
    }
}
