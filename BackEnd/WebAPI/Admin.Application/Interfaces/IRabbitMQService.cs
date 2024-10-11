using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Interfaces
{
    public interface IRabbitMQService
    {
        public void SendMessage(string queueName, string message);

        public void ReceiveMessage(string queueName, Action<string> messageHandler);
    }
}
