using Persistance.Context;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.DTOs.Products;

namespace Persistance.Repositories.Products
{
    internal class ProductRepository : IProductRepository
    {
        private ShopContext _shopContext;

        public ProductRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        // Create a new product
        public async Task<ProductDTO> CreateAsync(ProductDTO product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
            _shopContext.Products.Add(newProduct);
            await _shopContext.SaveChangesAsync();

            return new ProductDTO
            {
                ProductId = newProduct.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        // Read a product by ID
        public async Task<ProductDTO> ReadAsync(int productId)
        {
            var product = await _shopContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                return new ProductDTO
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };
            }

            return null;
        }

        // Update an existing product
        public async Task<ProductDTO> UpdateAsync(ProductDTO product)
        {
            var existingProduct = await _shopContext.Products.FirstOrDefaultAsync(p => p.Id == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                await _shopContext.SaveChangesAsync();

                return new ProductDTO
                {
                    ProductId = existingProduct.Id,
                    Name = existingProduct.Name,
                    Price = existingProduct.Price,
                    Description = existingProduct.Description
                };
            }

            return null;
        }

        // Delete a product by ID
        public async Task DeleteAsync(int productId)
        {
            var product = await _shopContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                _shopContext.Products.Remove(product);
                await _shopContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductListDTO>> ReadAllAsync()
        {
            return await _shopContext.Products.Select(p => new ProductListDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
            }).ToArrayAsync();
        }
    }
}
