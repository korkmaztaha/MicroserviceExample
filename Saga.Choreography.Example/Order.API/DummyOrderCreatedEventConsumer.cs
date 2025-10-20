using MassTransit;
using Shared.Events;

namespace Order.API
{
    public class DummyOrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            
            Console.WriteLine($"[DummyConsumer] Event received: {context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
