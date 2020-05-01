using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    Name = "TestProduct",
                    Type = "Software",
                    Price = 10.0M,
                    Discount = 30f
                },
                new Product
                {
                    ID = 2,
                    Name = "AnotherTestProduct",
                    Type = "Game",
                    Price = 14.990M,
                    Discount = 10f
                }
            );
        }
    }
}
