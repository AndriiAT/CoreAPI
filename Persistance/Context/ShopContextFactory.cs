using Microsoft.Extensions.Configuration;
using System.Data.Entity.Infrastructure;

namespace Persistance.Context
{
    internal class ShopContextFactory : IDbContextFactory<ShopContext>
    {
        private readonly IConfiguration _configuration;

        public ShopContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public ShopContext Create()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new ShopContext(connectionString);
        }
    }
}
