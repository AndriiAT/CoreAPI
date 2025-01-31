using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Persistance.DTOs;
using Persistance.Models;

namespace Persistance.Repositories
{
    internal class ServiceResultLogger : IServiceResultLogger
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceResultLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task LogResultAsync<T>(ServiceResultDTO<T> result, string methodName)
        {
            if (!result.IsSuccess)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                    var log = new ServiceResultLog
                    {
                        LogId = Guid.NewGuid().ToString(),
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                        Timestamp = DateTime.UtcNow,
                        MethodName = methodName
                    };
                    dbContext.ServiceResultLogs.Add(log);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
