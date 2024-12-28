using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Logging;

namespace Persistance.Context
{
    internal class ShopContextFactory : IDesignTimeDbContextFactory<ShopContext>
    {
        private readonly IConfiguration _configuration;

        public ShopContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public ShopContext CreateDbContext(string[] args)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ShopContext>();
            optionsBuilder
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            return new ShopContext(optionsBuilder.Options);
        }
    }
}
