using Persistance.Context;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.DTOs.Products;
using Persistance.DTOs;

namespace Persistance.Repositories.Products
{
    internal class ProductRepository : IProductRepository
    {
        private ShopDbContext _shopContext;

        public ProductRepository(ShopDbContext shopContext)
        {
            _shopContext = shopContext;
        }

        // Create a new product
        public async Task<ServiceResultDTO<ProductDTO>> CreateProductAsync(ProductDTO product)
        {
            var newProduct = new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
            _shopContext.Products.Add(newProduct);
            await _shopContext.SaveChangesAsync();

            return new ServiceResultDTO<ProductDTO>
            {
                IsSuccess = true,
                Data = new ProductDTO
                {
                    ProductId = newProduct.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                }
            };
        }

        // Read a product by ID
        public async Task<ServiceResultDTO<ProductDTO>> ReadProductAsync(string productId)
        {
            var product = await _shopContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                return new ServiceResultDTO<ProductDTO>
                {
                    IsSuccess = true,
                    Data = new ProductDTO
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description
                    }
                };
            }

            return new ServiceResultDTO<ProductDTO>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        }

        // Update an existing product
        public async Task<ServiceResultDTO<ProductDTO>> UpdateProductAsync(ProductDTO product)
        {
            var existingProduct = await _shopContext.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                await _shopContext.SaveChangesAsync();

                return new ServiceResultDTO<ProductDTO>
                {
                    IsSuccess = true,
                    Data = new ProductDTO
                    {
                        ProductId = existingProduct.ProductId,
                        Name = existingProduct.Name,
                        Price = existingProduct.Price,
                        Description = existingProduct.Description
                    }
                };
            }

            return new ServiceResultDTO<ProductDTO>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        }

        // Delete a product by ID
        public async Task<ServiceResultDTO<string>> DeleteProductAsync(string productId)
        {
            var product = await _shopContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                _shopContext.Products.Remove(product);
                await _shopContext.SaveChangesAsync();

                return new ServiceResultDTO<string>
                {
                    IsSuccess = true,
                    Data = "Product deleted succesfully"
                };
            }

            return new ServiceResultDTO<string>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        }

        public async Task<ServiceResultDTO<IEnumerable<ProductListDTO>>> ReadAllProductsAsync()
        {
            var products = await _shopContext.Products.Select(p => new ProductListDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
            }).ToListAsync();

            return new ServiceResultDTO<IEnumerable<ProductListDTO>>
            {
                IsSuccess = true,
                Data = products
            };
        }
    }
}
