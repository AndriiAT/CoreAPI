using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistance.Context
{
    internal class ShopContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        private readonly IConfiguration _configuration;

        public ShopContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public ShopDbContext CreateDbContext(string[] args)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionsBuilder
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            return new ShopDbContext(optionsBuilder.Options);
        }
    }
}
