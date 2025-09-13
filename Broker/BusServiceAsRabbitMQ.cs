using Order.Application;
using Order.Application.Order.CreateOrderUseCase;

namespace Broker
{
    public class BusServiceAsRabbitMQ : IBusService
    {
        public Task PublishAsync(OrderCreatedEvent orderCreatedEvent)
        {
            // Simulate publishing to a message broker
            Console.WriteLine($"Message gönderildi (RabbitMQ)");

            return Task.CompletedTask;
        }
    }
}
