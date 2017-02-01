using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Queue
{
    public class Send
    {
        public static void Main(string message, bool durable)
        {
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queue = "prion";
                var properties = channel.CreateBasicProperties();
                if (durable)
                {
                    queue = "prionDurable";
                    properties.Persistent = true;
                }

                channel.QueueDeclare(queue: queue,
                                     durable: durable,                                    
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
