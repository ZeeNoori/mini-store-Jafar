using System;
using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }


        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<OrderItem>()
                .HasKey(orderItem => new
                {
                    orderItem.OrderId,
                    orderItem.ProductId
                });

            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Product).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasPrecision(18, 2);

        }

    }
}
