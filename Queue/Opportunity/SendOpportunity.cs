using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Queue.Opportunity
{
    public class SendOpportunity
    {
        public void Send(string message)
        {

            var factory = new ConnectionFactory() { HostName = Main.Helpers.Settings.QueueHost, UserName = Main.Helpers.Settings.QueueUserName, Password = Main.Helpers.Settings.QueuePassword, VirtualHost = Main.Helpers.Settings.QueueVirtualHostQA };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                string queue = Main.Helpers.Settings.BwOpportunityQueue;

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                
                channel.QueueDeclare(queue: queue,
                                     durable: true,                                    
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

        public void SendToExchange(string message)
        {
            string exchange = Main.Helpers.Settings.QueueExchange;
            var routingKey = Main.Helpers.Settings.RoutingKey;

            var factory = new ConnectionFactory() { HostName = Main.Helpers.Settings.QueueHost, UserName = Main.Helpers.Settings.QueueUserName, Password = Main.Helpers.Settings.QueuePassword, VirtualHost = Main.Helpers.Settings.QueueVirtualHostQA };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }


    }
}
