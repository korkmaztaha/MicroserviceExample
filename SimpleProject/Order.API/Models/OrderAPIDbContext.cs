using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entites;

namespace Order.API.Models
{
    public class OrderAPIDbContext:DbContext
    {
        public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options)
        : base(options)
        {
        }




        public DbSet<Entites.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
