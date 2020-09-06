using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
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
