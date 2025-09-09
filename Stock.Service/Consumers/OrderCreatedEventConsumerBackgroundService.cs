using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.Service.Consumers
{
    public class OrderCreatedEventConsumerBackgroundService(IBus bus) : BackgroundService
    {
        private IModel? Channel { get; set; }
        // hook methods
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Channel = bus.GetChannel();

            // create queue
            Channel.QueueDeclare(queue: BusConst.StockOrderCreatedEventQueue,
                                 durable: true,
                                 exclusive: true,  // Bana özel
                                 autoDelete: false,
                                 arguments: null);

            // bind queue to exchange
            Channel.QueueBind(queue: BusConst.StockOrderCreatedEventQueue,
                              exchange: BusConst.OrderCreatedEventExchange,
                              routingKey: "",
                              null);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new EventingBasicConsumer(Channel);

            Channel.BasicConsume(queue: BusConst.StockOrderCreatedEventQueue,
                                autoAck: false,
                                consumer: consumer);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageAsJson = Encoding.UTF8.GetString(body);
                var orderCreedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(messageAsJson);

                Console.WriteLine($"Gelen Event : {orderCreedEvent.orderId}");

                

                Channel!.BasicAck(ea.DeliveryTag, false);


            };

            return Task.CompletedTask;


        }
    }
}
