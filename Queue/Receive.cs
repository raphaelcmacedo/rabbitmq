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

        public static void Main()
        {

            listen = true;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "prion",
                                        durable: false,
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
                channel.BasicConsume(queue: "prion",
                                        noAck: true,
                                        consumer: consumer);

                /*
                System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\logs\\rabbit_log.txt");
                file.WriteLine(consumer.Model.ToString());
                file.Close();
                */
                
                while (listen)
                { }

                /*
                file = new System.IO.StreamWriter("d:\\logs\\rabbit_log.txt");
                file.WriteLine("Deu ruim");
                file.Close();
                */
                
            }
        }

        public static string GetOneMessage(bool simulateError = false, bool simulateRejection = false)
        {
            string message = string.Empty;
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {


                bool noAck = false;
                BasicGetResult result = channel.BasicGet("prion", noAck);
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