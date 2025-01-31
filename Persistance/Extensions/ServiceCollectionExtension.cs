using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistance.Context;
using Persistance.Models;
using Persistance.Repositories;
using Persistance.Repositories.Accounts;
using Persistance.Repositories.Orders;
using Persistance.Repositories.Products;
using Persistance.Services;
using Persistance.Services.Accounts;
using ProductsShop.Repositories.Accounts;
using System.Security.Claims;

namespace Persistance.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopDbContext>((serviceProvider, optionsBuilder) =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ShopDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IServiceResultLogger, ServiceResultLogger>();
            services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();

            services.AddSingleton<IListsComparer, ListsComparer>();

            return services;
        }
    }

    public interface IRoleSeeder
    {
        Task SeedRolesAndClaims();
    }

    internal class RoleSeeder(RoleManager<ApplicationRole> roleManager) : IRoleSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public async Task SeedRolesAndClaims()
        {
            var roleName = "Admin";
            var claims = new[]
            {
                new Claim("Permission", "Read"),
                new Claim("Permission", "Write"),
                new Claim("Permission", "Full")
            };

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new ApplicationRole();
                await _roleManager.CreateAsync(role);
            }

            foreach (var claim in claims)
            {
                if (!(await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    await _roleManager.AddClaimAsync(role, claim);
                }
            }
        }
    }

    public interface IUserSeeder
    {
        Task SeedUsersAndClaims();
    }

    internal class UserSeeder(UserManager<ApplicationUser> userManager) : IUserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task SeedUsersAndClaims()
        {
            var userEmail = "admin@example.com";
            var claims = new[]
            {
                new Claim("Permission", "Read"),
                new Claim("Permission", "Write")
            };

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfCreation = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                };
                await _userManager.CreateAsync(user, "Password123!");
            }

            foreach (var claim in claims)
            {
                if (!(await _userManager.GetClaimsAsync(user)).Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    await _userManager.AddClaimAsync(user, claim);
                }
            }
        }
    }
}
