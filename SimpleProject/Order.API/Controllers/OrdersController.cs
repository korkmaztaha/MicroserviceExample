using MassTransit;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entites;
using Order.API.ViewModels;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            Models.Entites.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = createOrder.BuyerId,
                CreatedDate = DateTime.Now,
                OrderStatu = Models.Enums.OrderStatus.Suspended,
            };
            order.OrderItems = createOrder.OrderItems.Select(oi => new OrderItem
            {
                Count = oi.Count,
                Price = oi.Price,
                ProductId =  oi.ProductId.ToString(),
            }).ToList();

            order.TotalPrice = createOrder.OrderItems.Sum(oi => oi.Price * oi.Count);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.OrderId,
                BuyerId = order.BuyerId,
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
                {
                    ProductId = oi.ProductId,
                    Count = oi.Count,
                }).ToList(),
                TotalPrice = order.TotalPrice,
            };
            //yayınlandığında sub olan tüm servisler bu eventten haberdar olacak. Event bazlı gönderim.
            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok();

        }
    }
}
