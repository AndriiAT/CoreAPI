using Persistance.DTOs;

namespace Persistance.Repositories
{
    public interface IOrderedProductRepository
    {
        Task<OrderedProductDTO> CreateAsync(OrderedProductDTO product);
        Task<OrderedProductDTO> ReadAsync(int productId);
        Task<OrderedProductDTO> UpdateAsync(OrderedProductDTO product);
        Task DeleteAsync(int productId);
        Task<IEnumerable<OrderedProductDTO>> ReadAllAsync(int orderId);
    }
}
