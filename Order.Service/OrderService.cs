using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Order.Service.Services;
using ServiceBus;

namespace Order.Service
{
    public class OrderService(ServiceBus.IBus bus, IPublishEndpoint publishEndpoint, StockService stockService):IOrderService
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

            var result = await stockService.CheckStockAsync(1, 5);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(60));  // 60 saniye sonra iptal et.

            await publishEndpoint.Publish(orderCreatedEvent, pipeline => {
                pipeline.SetAwaitAck(true);
                pipeline.Durable = true;
            },cancellationTokenSource.Token);  // retry mekanizması var.

        }   

    }
}
