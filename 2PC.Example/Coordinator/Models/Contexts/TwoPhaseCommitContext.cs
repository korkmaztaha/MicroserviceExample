using Microsoft.EntityFrameworkCore;

namespace Coordinator.Models.Contexts
{
    public class TwoPhaseCommitContext:DbContext
    {
        public TwoPhaseCommitContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeState> NodeStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>()
         .HasData(
             new Node("Order.API")
             {
                 Id = SeedGuids.OrderApiNodeId 
             },
             new Node("Payment.API")
             {
                 Id = SeedGuids.PaymentApiNodeId
             },
             new Node("Stock.API")
             {
                 Id = SeedGuids.StockApiNodeId 
             }
         );



        }

        public static class SeedGuids
        {
            
            public static readonly Guid OrderApiNodeId = new Guid("10000000-0000-0000-0000-000000000001");
            public static readonly Guid PaymentApiNodeId = new Guid("20000000-0000-0000-0000-000000000002");
            public static readonly Guid StockApiNodeId = new Guid("30000000-0000-0000-0000-000000000003");

         
        }

    }
}
