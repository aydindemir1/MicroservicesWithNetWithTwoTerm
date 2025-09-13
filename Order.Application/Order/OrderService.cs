using Order.Application.Order.CreateOrderUseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Order
{
    

    public class OrderService(IOrderRepository orderRepository, IBusService busService): IOrderService
    {
       async Task<CreateOrderResponse> IOrderService.Create(CreateOrderRequest request)
        {
            var order = new Domain.Order
            {
                Name = request.Name,
                Quantity = request.Quantity,
                Price = request.Price
            };
            var orderId = orderRepository.CreateOrder(order);
            
            await busService.PublishAsync(new OrderCreatedEvent(orderId, order.Quantity));
            return new CreateOrderResponse(orderId);

        }

    }
}
