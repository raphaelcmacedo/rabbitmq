using Main.Services;
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
            string queue = "ha.bwsalesopportunity.queue";
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "qa" };
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
                    HandleSalesData(channel, model, ea);
                };
                channel.BasicConsume(queue: queue,
                                        noAck: false,
                                        consumer: consumer);

                while (listen)
                {}
                
            }
        }

        private void HandleSalesData(IModel channel, object model, BasicDeliverEventArgs ea)
        {
            string salesDataMessage = "";
            try
            {
                var body = ea.Body;
                //Get sales data message (sap message)
                salesDataMessage = Encoding.UTF8.GetString(body);
                //Save sales data on DB and get opportunity message
                Main.Services.OpportunityIntegration integration = new Main.Services.OpportunityIntegration();
                string opportunityMessage = integration.CreateOpportunity(salesDataMessage);
                //Send opportunity message to rabbitmq
                SendOpportunity send = new SendOpportunity();
                send.Send(opportunityMessage);
                //Send ack to queue
                //channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                channel.BasicReject(ea.DeliveryTag, false);
                FreshDesk.GenerateTicket(salesDataMessage, e.Message);
            }
          

        }

        public void CreateOpportunityListener()
        {
            listen = true;
            string queue = "ha.bwopportunity.queue";
            var factory = new ConnectionFactory() { HostName = "DV0219", UserName = "queue_user", Password = "testing1", VirtualHost = "qa" };
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
                    //Get opportunity message
                    string message = Encoding.UTF8.GetString(body);
                    //Send opportunity to sales force
                    Main.Services.OpportunityIntegration integration = new Main.Services.OpportunityIntegration();
                    integration.CreateSalesForceOpportunity(message);
                    //Send ack to queue
                    //channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: queue,
                                        noAck: false,
                                        consumer: consumer);

                while (listen)
                { }

            }
        }

    }
}