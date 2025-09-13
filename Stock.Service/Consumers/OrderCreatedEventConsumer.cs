using MassTransit;
using ServiceBus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Service.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {

            // event
            // db

            var hasStock = true;

            if(hasStock) {
               
            }
            else
            {

            }

            // db
            //


            Console.WriteLine("Consume Methodu Çalıştı");

            throw new DBConcurrencyException();

            Console.WriteLine($"(MassTransit) Gelen Event : {context.Message.orderId}");
            
            return Task.CompletedTask;
        }
    }
}
