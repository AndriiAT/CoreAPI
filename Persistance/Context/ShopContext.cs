
using Microsoft.EntityFrameworkCore;
using Persistance.Models;

namespace Persistance.Context
{
    internal class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderedProduct> OrderedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderedProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);
        }
    }
}
