using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Models;

namespace Persistance.Context
{
    internal class ShopDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        public DbSet<ServiceResultLog> ServiceResultLogs { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial roles and users
            var adminRoleId = "admin-role-id";
            var userRoleId = "user-role-id";
            var managerRoleId = "manager-role-id";

            var adminUserId = Guid.NewGuid().ToString();
            var userUserId = Guid.NewGuid().ToString();
            var managerUserId = Guid.NewGuid().ToString();

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role",
                    CreationDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "User role",
                    CreationDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    Description = "Manager role",
                    CreationDate = DateTime.UtcNow
                }
            );

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = adminUserId,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin"),
                    SecurityStamp = string.Empty,
                    FirstName = "Admin",
                    LastName = "Test",
                    RoleName = "Admin"
                },
                new ApplicationUser
                {
                    Id = userUserId,
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@example.com",
                    NormalizedEmail = "USER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "user"),
                    SecurityStamp = string.Empty,
                    FirstName = "User",
                    LastName = "Test",
                    RoleName = "User"
                },
                new ApplicationUser
                {
                    Id = managerUserId,
                    UserName = "manager",
                    NormalizedUserName = "MANAGER",
                    Email = "manager@example.com",
                    NormalizedEmail = "MANAGER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "manager"),
                    SecurityStamp = string.Empty,
                    FirstName = "Manager",
                    LastName = "Test",
                    RoleName = "Manager"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = userUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = managerRoleId,
                    UserId = managerUserId
                }
            );

            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderedProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderedProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<ServiceResultLog>()
                .HasKey(srl => srl.LogId);

            modelBuilder.Entity<UserLoginLog>()
                .HasKey(log => log.LogId);

            // Seed initial products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Phone", Price = 150, Description = "Description for Phone" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "IPhone", Price = 350, Description = "Description for IPhone" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Android Phone", Price = 250, Description = "Description for Android Phone" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Android used", Price = 50, Description = "Description for Android used" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "HP PC", Price = 1500, Description = "Description for HP PC" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Notebook", Price = 8500, Description = "Description for Notebook" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Head Phones", Price = 50, Description = "Description for Head Phones" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Mouse", Price = 20, Description = "Description for Mouse" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Monitor 24", Price = 175, Description = "Description for Monitor 24" },
                new Product { ProductId = Guid.NewGuid().ToString(), Name = "Monitor 27", Price = 230, Description = "Description for Monitor 27" }
            );
        }
    }
}
