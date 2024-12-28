using Persistance.DTOs;

namespace Persistance.Repositories
{
    public interface IOrderRepository
    {
        Task<ServiceResultDTO<OrderDTO>> CreateAsync(OrderDTO order);
        Task<OrderDTO> ReadAsync(int orderId);
        Task<ServiceResultDTO<OrderDTO>> UpdateAsync(OrderDTO order);
        Task DeleteAsync(int orderId);
        Task<IEnumerable<OrderDTO>> ReadAllAsync();
    }
}
