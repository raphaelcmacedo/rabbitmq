using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Queue.Opportunity
{

    public class ReceiveOpportunity
    {
        public bool listen { get; set; }
        
        public void CreateSalesDataListener()
        {
            
            listen = true;
            string exchange = "";
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "qa" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                              exchange: exchange,
                              routingKey: "");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;

                    //Get sales data message (sap message)
                    string salesDataMessage = Encoding.UTF8.GetString(body);
                    //Save sales data on DB and get opportunity message
                    Main.Services.OpportunityIntegration integration = new Main.Services.OpportunityIntegration();
                    string opportunityMessage = integration.CreateOpportunity(salesDataMessage);
                    //Send opportunity message to rabbitmq

                };
                channel.BasicConsume(queue: queueName,
                                        noAck: true,
                                        consumer: consumer);


                while (listen)
                { }
                
            }
        }
        
    }
}