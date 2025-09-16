using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models.Entites;
using Order.API.ViewModels;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            Models.Entites.Order order = new ()
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
               ProductId = oi.ProductId,    
            }).ToList();

            order.TotalPrice = createOrder.OrderItems.Sum(oi => oi.Price);

            return Ok();

        }   
    }
}
