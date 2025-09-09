using Microsoft.AspNetCore.Http.HttpResults;
using ServiceBus;

namespace Order.Service
{
    public class OrderService(IBus bus):IOrderService
    {
        public async Task Create()
        {

            var orderCreatedEvent = new OrderCreatedEvent(10, new Dictionary<int, int>
            {
                { 1, 5 },
                { 2, 5 }
            });

            await bus.Send(orderCreatedEvent, BusConst.OrderCreatedEventExchange);


           
        }   

    }
}
