using Persistance.DTOs.Products;

namespace Persistance.Repositories.Products
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
