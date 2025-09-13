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
                                 exclusive: false,  // Bana özel değil : false , Bana özel : true
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



            //ConnectionMultiplexer.shutdown : Redis  bağklantısını kontrol eder, Redis ayakta mı kontrol eder.

            Channel.BasicConsume(queue: BusConst.StockOrderCreatedEventQueue,
                                autoAck: false,  // mesajı aldım demek // False :  Ben seni haberdar etmek istiyorum mesajlar ıaldım  ve işledim diye<, True :  mesajları aldım ve işledim demek istemiyorum. Sen Msajları sil
                                consumer: consumer);

            Channel!.CallbackException += Channel_CallbackException;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageAsJson = Encoding.UTF8.GetString(body);
                var OrderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(messageAsJson);



                Console.WriteLine($"Gelen Event : {OrderCreatedEvent.orderId}");

                Channel!.BasicAck(ea.DeliveryTag, false); //  : mesaj işlendi demek, false : tek bir mesaj işlendi demek, true :  o ana kadar gelen tüm mesajlar işlendi demek


            };

            return Task.CompletedTask;


        }

        private void Channel_CallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
