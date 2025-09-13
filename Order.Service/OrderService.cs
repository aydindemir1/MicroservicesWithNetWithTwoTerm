using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using ServiceBus;

namespace Order.Service
{
    public class OrderService(ServiceBus.IBus bus, IPublishEndpoint publishEndpoint):IOrderService
    {
        public async Task Create()
        {

            var orderCreatedEvent = new OrderCreatedEvent(10, new Dictionary<int, int>
            {
                { 1, 5 },
                { 2, 5 }
            });

            //await bus.Send(orderCreatedEvent, BusConst.OrderCreatedEventExchange);

            //cancelation token => iptal edilebilir operasyonlar için kullanılır.

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(60));  // 60 saniye sonra iptal et.

            await publishEndpoint.Publish(orderCreatedEvent, pipeline => {
                pipeline.SetAwaitAck(true);
                pipeline.Durable = true;
            },cancellationTokenSource.Token);  // retry mekanizması var.

        }   

    }
}
