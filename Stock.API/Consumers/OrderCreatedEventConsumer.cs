using MassTransit;
using Shared.Events;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            Console.WriteLine($"OrderCreatedEvent consumed: {context.Message.BuyerId}");
        }
    }
}
