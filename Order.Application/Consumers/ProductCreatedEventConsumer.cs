using MassTransit;
using Order.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Consumers
{
    public class ProductCreatedEventConsumer(ISyncWriteRepository syncWriteRepository) : IConsumer<ProductCreatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var productCreatedEvent = context.Message;

            await syncWriteRepository.Create(new Domain.Read.ProductWithCategory()
            {
                Id = productCreatedEvent.Id,
                Name = productCreatedEvent.Name,
                Price = productCreatedEvent.Price,
                Quantity = productCreatedEvent.Quantity,
                CategoryName = productCreatedEvent.CategoryName
            });

            
        }
    }
}
