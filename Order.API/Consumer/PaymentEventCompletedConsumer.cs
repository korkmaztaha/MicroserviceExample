using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Order.API.Models.Entites;
using Shared.Events;

namespace Order.API.Consumer
{
    public class PaymentEventCompletedConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _dbContext;

        public PaymentEventCompletedConsumer(OrderAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Models.Entites.Order order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
            order.OrderStatu = Models.Enums.OrderStatus.Completed;
        }
    }
}
