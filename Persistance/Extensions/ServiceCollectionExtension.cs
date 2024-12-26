using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Persistance.Repositories;

namespace Persistance.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services)
        {
            services.AddScoped<ShopContext>(_ =>
            {
                var configuration = _.GetService<IConfiguration>();
                return new ShopContext(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
