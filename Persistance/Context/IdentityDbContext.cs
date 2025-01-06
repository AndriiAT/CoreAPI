using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Models;

namespace Persistance.Context
{
    internal class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed initial admin role and user
            var adminRoleId = "admin-role-id";
            var adminUserId = "admin-user-id";

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role",
                CreatedDate = DateTime.UtcNow
            });

            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
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
                LastName = "User"
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}
