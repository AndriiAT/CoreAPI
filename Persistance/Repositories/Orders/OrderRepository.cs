using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Persistance.DTOs;
using Persistance.DTOs.Orders;
using Persistance.DTOs.Products;
using Persistance.Models;
using Persistance.Services;
using static Persistance.DTOs.Orders.Enums;

namespace Persistance.Repositories.Orders
{
    internal class OrderRepository(
        ShopContext _shopContext,
        IListsComparer _listComparer
    ) : IOrderRepository
    {
        public async Task<ServiceResultDTO<OrderDTO>> CreateAsync(OrderDTO order)
        {
            var productIds = order.OrderedProducts.Select(op => op.ProductId).ToList();
            var products = await _shopContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            if (products.Count != productIds.Count)
            {
                return new ServiceResultDTO<OrderDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "One or more orderedProducts do not exist",
                };
            }

            var newOrder = new Order
            {
                UserId = order.UserId,
                OrderedProducts = order.OrderedProducts.Select(p => new OrderedProduct
                {
                    ProductId = p.ProductId,
                    Price = products.First(pr => pr.Id == p.ProductId).Price,
                    Count = p.Count,
                }).ToList(),
                Status = order.Status,
                AddDate = order.AddDate,
                ModifiedDate = order.ModifiedDate
            };

            _shopContext.Orders.Add(newOrder);
            await _shopContext.SaveChangesAsync();

            return new ServiceResultDTO<OrderDTO>
            {
                IsSuccess = true,
                Data = await ReadAsync(newOrder.Id),
            };
        }

        public async Task<OrderDTO> ReadAsync(int orderId)
        {
            var order = await _shopContext.Orders
                .Select(order => new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderedProducts = order.OrderedProducts.Select(op => new OrderedProductDTO
                    {
                        Id = op.Id,
                        OrderId = orderId,
                        Name = op.Product.Name,
                        ProductId = op.ProductId,
                        Price = op.Price,
                        Count = op.Count
                    }).ToList(),
                    Status = order.Status,
                    AddDate = order.AddDate,
                    ModifiedDate = order.ModifiedDate
                })
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order;
        }

        public async Task<ServiceResultDTO<OrderDTO>> UpdateAsync(OrderDTO orderDto)
        {
            var productIds = orderDto.OrderedProducts.Select(op => op.ProductId).ToList();
            var existingProducts = await _shopContext.Products.Where(op => productIds.Contains(op.Id)).ToListAsync();

            if (existingProducts.Count != productIds.Count)
            {
                return new ServiceResultDTO<OrderDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "One or more products do not exist",
                };
            }

            var existingOrder = await _shopContext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderDto.Id);

            if (existingOrder == null)
            {
                return new ServiceResultDTO<OrderDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = "Order not found",
                };
            }

            var existingOrderedProducts = await _shopContext.OrderedProducts
                .Where(op => op.OrderId == orderDto.Id)
                .ToListAsync();

            var orderedProducts = orderDto.OrderedProducts.Select(p => new OrderedProduct
            {
                Id = p.Id,
                OrderId = orderDto.Id,
                ProductId = p.ProductId,
                Count = p.Count,
            }).ToList();

            var compareResult = _listComparer.Compare(existingOrderedProducts, orderedProducts, op => op.Id);
            // add new ordered products
            foreach (var added in compareResult.Added)
            {
                added.OrderId = orderDto.Id;
                added.Price = existingProducts.First(p => p.Id == added.ProductId).Price;
            }
            _shopContext.OrderedProducts.AddRange(compareResult.Added);
            // remove ordered products
            _shopContext.OrderedProducts.RemoveRange(compareResult.Removed);
            // update ordered products
            foreach (var modified in compareResult.Updated)
            {
                var product = orderedProducts.First(op => op.Id == modified.Id);
                modified.Count = product.Count;
            }
            // update order
            existingOrder.Status = OrderStatus.Updated;
            existingOrder.ModifiedDate = DateTime.UtcNow;

            await _shopContext.SaveChangesAsync();

            return new ServiceResultDTO<OrderDTO>
            {
                IsSuccess = true,
                Data = await ReadAsync(orderDto.Id),
            };
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _shopContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _shopContext.Orders.Remove(order);
                await _shopContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderDTO>> ReadAllAsync()
        {
            var orders = await _shopContext.Orders
                .Select(order => new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderedProducts = order.OrderedProducts.Select(op => new OrderedProductDTO
                    {
                        Id = op.Id,
                        OrderId = op.OrderId,
                        ProductId = op.ProductId,
                        Name = op.Product.Name,
                        Price = op.Price,
                        Count = op.Count
                    }).ToList(),
                    Status = order.Status,
                    AddDate = order.AddDate,
                    ModifiedDate = order.ModifiedDate,
                })
                .ToListAsync();

            return orders;
        }
    }
}
