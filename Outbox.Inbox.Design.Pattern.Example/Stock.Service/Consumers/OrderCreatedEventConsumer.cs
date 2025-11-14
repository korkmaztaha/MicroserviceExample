using MassTransit;
using Shared.Events;
using Stock.Service.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.Service.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext stockDbContext) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            Console.WriteLine($"[Stock Service] OrderCreatedEvent consumed: {JsonSerializer.Serialize(context.Message)}");
        }
    }
}