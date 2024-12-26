using Persistance.DTOs;

namespace Persistance.Repositories
{
    public interface IProductRepository
    {
        Task<ProductDTO> CreateAsync(ProductDTO product);
        Task<ProductDTO> ReadAsync(int productId);
        Task<ProductDTO> UpdateAsync(ProductDTO product);
        Task DeleteAsync(int productId);
        Task<IEnumerable<ProductListDTO>> ReadAllAsync();
    }
}
