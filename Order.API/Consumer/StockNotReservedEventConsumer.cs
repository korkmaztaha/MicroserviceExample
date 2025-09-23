using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly OrderAPIDbContext _dbContext;

        public StockNotReservedEventConsumer(OrderAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Models.Entites.Order order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
            order.OrderStatu = Models.Enums.OrderStatus.Failed;
            await _dbContext.SaveChangesAsync();
        }
    }
}
