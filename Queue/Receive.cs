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

        public static void CreateListener(bool durable)
        {
            string queue = "prion";
            if (durable)
            {
                queue = "prionDurable";
            }
            listen = true;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                        durable: true,
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

        public static string GetOneMessage(bool durable, bool simulateError = false, bool simulateRejection = false)
        {
            string queue = "prion";
            if (durable)
            {
                queue = "prionDurable";
            }

            string message = string.Empty;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
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
                if (simulateRejection)
                {
                    channel.BasicReject(result.DeliveryTag, false);
                    throw new Exception("The message " + message + " has been rejected.");
                }
                else
                {
                    channel.BasicAck(result.DeliveryTag, false);
                }
                
            }

            return message;
        }
    }
}