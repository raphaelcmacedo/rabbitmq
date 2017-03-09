using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Queue;

namespace Queue
{
    public class Send
    {
        public static void Main(string message, string queue, bool durable)
        {

            Queue.Opportunity.ReceiveOpportunity RO = new Opportunity.ReceiveOpportunity();
            RO.CreateSalesDataListener();

            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "dev"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                queue = Util.HandleQueueName(queue, durable);
                var properties = channel.CreateBasicProperties();
                if (durable)
                {
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

        public static void SendToExchange(string message, string exchange)
        {
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "dev" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: "fanout", durable: true);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchange,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }


    }
}
