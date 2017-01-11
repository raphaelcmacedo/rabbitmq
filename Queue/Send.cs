using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Queue
{
    public class Send
    {
        public static void Main(string message)
        {
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "prion",
                                     durable: false,                                    
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "prion",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
