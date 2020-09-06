using Microsoft.EntityFrameworkCore;
using WebMarket.Models;

namespace WebMarket.Data
{
    public class MainDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<UserComment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
