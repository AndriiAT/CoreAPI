using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistance.Context;
using Persistance.Repositories;
using Persistance.Services;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistance.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services)
        {
            services.AddDbContext<ShopContext>((serviceProvider, optionsBuilder) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderedProductRepository, OrderedProductRepository>();

            services.AddSingleton<IListsComparer, ListsComparer>();

            return services;
        }
    }
}
