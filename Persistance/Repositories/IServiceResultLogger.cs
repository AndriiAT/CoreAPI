using Persistance.DTOs;

namespace Persistance.Repositories
{
    internal interface IServiceResultLogger
    {
        Task LogResultAsync<T>(ServiceResultDTO<T> result, string methodName);
    }
}
