using Persistance.DTOs;
using Persistance.DTOs.Orders;

namespace Persistance.Repositories
{
    public interface IOrderRepository
    {
        Task<ServiceResultDTO<OrderDTO>> CreateOrderAsync(OrderDTO order);
        Task<OrderDTO> ReadOrderAsync(string userId, string orderId);
        Task<ServiceResultDTO<OrderDTO>> UpdateOrderAsync(string userId, OrderDTO order);
        Task<ServiceResultDTO<string>> DeleteOrderAsync(string orderId);
        Task<ServiceResultDTO<IEnumerable<OrderDTO>>> ReadUserOrdersAsync(string userId);
        Task<ServiceResultDTO<IEnumerable<OrderDTO>>> ReadUserOrdersAsync();
    }
}
