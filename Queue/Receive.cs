using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Queue
{

    public class Receive
    {
        public static string message { get; set; }
        public static bool listen { get; set; }

        public static void CreateListener(string queue, bool durable)
        {
            queue = Util.HandleQueueName(queue, durable);
            listen = true;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "dev" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                        durable: durable,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    if (!string.IsNullOrEmpty(message))
                    {
                        message += "\r\n";
                    }
                    message += Encoding.UTF8.GetString(body);
                };
                channel.BasicConsume(queue: queue,
                                        noAck: true,
                                        consumer: consumer);

                
                while (listen)
                { }

                
                
            }
        }

        public static void CreateListenerExchange(string exchange)
        {
            
            listen = true;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "dev" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: "fanout", durable: true);
                var queueName = channel.QueueDeclare().QueueName;
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    if (!string.IsNullOrEmpty(message))
                    {
                        message += "\r\n";
                    }
                    message += Encoding.UTF8.GetString(body);
                };
                channel.BasicConsume(queue: queueName,
                                        noAck: true,
                                        consumer: consumer);


                while (listen)
                { }



            }
        }

        public static string GetOneMessage(bool durable, string queue = "ha.prion", string virtualHost = "dev", bool simulateError = false, bool simulateRejection = false)
        {
            queue = Util.HandleQueueName(queue, durable);

            string message = string.Empty;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = virtualHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {


                bool noAck = false;
                BasicGetResult result = channel.BasicGet(queue, noAck);
                if (result == null)
                {
                    throw new Exception("Queue is empty.");
                }
                else {
                    IBasicProperties props = result.BasicProperties;
                    byte[] body = result.Body;
                    message = System.Text.Encoding.UTF8.GetString(body);
                    if (simulateError)
                    {
                        throw new Exception("Simulated error. The ack has not been sent.");
                    }
                }
                
                // acknowledge receipt of the message
                /*if (simulateRejection)
                {
                    channel.BasicReject(result.DeliveryTag, false);
                    throw new Exception("The message " + message + " has been rejected.");
                }
                else
                {
                    channel.BasicAck(result.DeliveryTag, false);
                }*/
                
            }

            return message;
        }
    }
}