﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Order.API.Models.Contexts
{
    public class OrderAPIDbContext : DbContext
    {
        public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}