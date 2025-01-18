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
using ProductsShop.Repositories.Accounts;
using IdentityDbContext = Persistance.Context.IdentityDbContext;

namespace Persistance.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopContext>((serviceProvider, optionsBuilder) =>
            {
                //var configuration = serviceProvider.GetService<IConfiguration>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            });

            services.AddDbContext<IdentityDbContext>((serviceProvider, optionsBuilder) =>
            {
                //var configuration = serviceProvider.GetService<IConfiguration>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            });

            //services.AddDbContext<IdentityDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            //var jwtSettings = configuration.GetSection("JwtSettings");
            //var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = jwtSettings["Issuer"],
            //        ValidAudience = jwtSettings["Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(key)
            //    };
            //});

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderedProductRepository, OrderedProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddSingleton<IListsComparer, ListsComparer>();

            return services;
        }
    }
}
