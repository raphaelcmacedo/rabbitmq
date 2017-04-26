﻿using Main.Services;
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
            string queue = Main.Helpers.Settings.SalesDataQueue; 
            var factory = new ConnectionFactory() { HostName = Main.Helpers.Settings.QueueHost, UserName = Main.Helpers.Settings.QueueUserName, Password = Main.Helpers.Settings.QueuePassword, VirtualHost = Main.Helpers.Settings.QueueVirtualHostQA };
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
                send.SendToExchange(opportunityMessage);
                //Send ack to queue
                channel.BasicAck(ea.DeliveryTag, false);
                
            }
            catch (Exception e)
            {
                //Send ack
                channel.BasicReject(ea.DeliveryTag, false);
                //Create ticket
                Email.SendEmail(salesDataMessage, e.Message);
            }          

        }

        public void CreateOpportunityListener()
        {
            listen = true;
            string queue = Main.Helpers.Settings.OpportunityQueue;
            var factory = new ConnectionFactory() { HostName = Main.Helpers.Settings.QueueHost, UserName = Main.Helpers.Settings.QueueUserName, Password = Main.Helpers.Settings.QueuePassword, VirtualHost = Main.Helpers.Settings.QueueVirtualHostQA };
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
                    HandleOpportunity(channel, model, ea);
                };
                channel.BasicConsume(queue: queue,
                                        noAck: false,
                                        consumer: consumer);

                while (listen)
                { }

            }
        }

        private void HandleOpportunity(IModel channel, object model, BasicDeliverEventArgs ea)
        {
            string message = "";
            try
            {
                var body = ea.Body;
                //Get opportunity message
                message = Encoding.UTF8.GetString(body);
                //Send opportunity to sales force
                Main.Services.OpportunityIntegration integration = new Main.Services.OpportunityIntegration();
                integration.CreateSalesForceOpportunity(message);

                //Send ack to queue
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Send ack
                channel.BasicReject(ea.DeliveryTag, false);
                //Create ticket
                Email.SendEmail(message, e.Message);
            }
        }

        public void TesteSalesForce()
        {
            Main.SalesForceIntegration.SalesForceService service = new Main.SalesForceIntegration.SalesForceService();
            Main.SalesForceSVC.QueryResult res = service.FindUserByUsername("tim.hare2@westcon.com");
            Console.WriteLine(res.records.Length);
        }

    }
}