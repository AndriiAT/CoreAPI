using Persistance.DTOs;
using Persistance.DTOs.Products;

namespace Persistance.Repositories.Products
{
    public interface IProductRepository
    {
        Task<ServiceResultDTO<ProductDTO>> CreateProductAsync(ProductDTO product);
        Task<ServiceResultDTO<ProductDTO>> ReadProductAsync(string productId);
        Task<ServiceResultDTO<ProductDTO>> UpdateProductAsync(ProductDTO product);
        Task<ServiceResultDTO<string>> DeleteProductAsync(string productId);
        Task<ServiceResultDTO<IEnumerable<ProductListDTO>>> ReadAllProductsAsync();
    }
}
