using Persistance.Context;
using Persistance.DTOs;
using Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories
{
    internal class OrderedProductRepository : IOrderedProductRepository
    {
        private ShopContext _shopContext;

        public OrderedProductRepository(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<OrderedProductDTO> CreateAsync(OrderedProductDTO product)
        {
            var orderedProduct = new OrderedProduct
            {
                // Map properties from DTO to entity
                Id = product.Id,
                OrderId = product.OrderId,
                Product = await _shopContext.Products.FirstAsync(p => p.Id == product.Id),
                Price = product.Price,
                Count = product.Count
            };

            _shopContext.OrderedProducts.Add(orderedProduct);
            await _shopContext.SaveChangesAsync();

            return new OrderedProductDTO
            {
                Id = orderedProduct.Id,
                OrderId = orderedProduct.OrderId,
                Name = orderedProduct.Product.Name,
                Price = orderedProduct.Price,
                Count = orderedProduct.Count 
            };
        }

        public async Task<OrderedProductDTO> ReadAsync(int productId)
        {
            var orderedProduct = await _shopContext.OrderedProducts
                .Include(op => op.Product)
                .FirstOrDefaultAsync(op => op.Id == productId);

            if (orderedProduct == null)
                return null;

            return new OrderedProductDTO
            {
                Id = orderedProduct.Id,
                OrderId = orderedProduct.OrderId,
                Name = orderedProduct.Product.Name,
                Price = orderedProduct.Price,
                Count = orderedProduct.Count
            };
        }

        public async Task<OrderedProductDTO> UpdateAsync(OrderedProductDTO product)
        {
            var orderedProduct = await _shopContext.OrderedProducts
                .Include(op => op.Product)
                .FirstOrDefaultAsync(op => op.Id == product.Id);

            if (orderedProduct == null)
                return null;

            orderedProduct.Product.Name = product.Name;
            orderedProduct.Price = product.Price;
            orderedProduct.Count = product.Count;

            await _shopContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int productId)
        {
            var orderedProduct = await _shopContext.OrderedProducts
                .FirstOrDefaultAsync(op => op.Id == productId);

            if (orderedProduct != null)
            {
                _shopContext.OrderedProducts.Remove(orderedProduct);
                await _shopContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderedProductDTO>> ReadAllAsync(int orderId)
        {
            return await _shopContext.OrderedProducts
                .Include(op => op.Product)
                .Select(op => new OrderedProductDTO
                {
                    Id = op.Id,
                    OrderId = orderId,
                    Name = op.Product.Name,
                    Price = op.Price,
                    Count = op.Count
                })
                .ToListAsync();
        }
    }
}
