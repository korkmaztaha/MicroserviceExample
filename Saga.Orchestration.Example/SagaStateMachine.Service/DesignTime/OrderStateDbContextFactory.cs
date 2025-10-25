using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SagaStateMachine.Service.StateDbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachine.Service.DesignTime
{
    public class OrderStateDbContextFactory : IDesignTimeDbContextFactory<OrderStateDbContext>
    {
        public OrderStateDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json")              
                .Build();

            // connection string'i oku
            var connectionString = configuration.GetConnectionString("SQLServer");

            var optionsBuilder = new DbContextOptionsBuilder<OrderStateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OrderStateDbContext(optionsBuilder.Options);
        }
    }
}
