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
    internal class OrderRepository : IOrderRepository
    {
        
        private readonly ShopDbContext _shopContext;
        private readonly IServiceResultLogger _logger;
        private readonly IListsComparer _listComparer;

        public OrderRepository(ShopDbContext shopContext, IServiceResultLogger logger, IListsComparer listComparer)
        {
            _shopContext = shopContext;
            _logger = logger;
            _listComparer = listComparer;
        }

        public async Task<ServiceResultDTO<OrderDTO>> CreateOrderAsync(OrderDTO order)
        {
            var result = new ServiceResultDTO<OrderDTO>();
            try
            {
                var productIds = order.OrderedProducts.Select(op => op.ProductId).ToList();
                var products = await _shopContext.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

                if (products.Count != productIds.Count)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "One or more ordered products do not exist";
                    return result;
                }

                var newOrderId = Guid.NewGuid().ToString();
                var orderEntity = new Order
                {
                    OrderId = newOrderId,
                    UserId = order.UserId,
                    OrderedProducts = order.OrderedProducts.ConvertAll(p => new OrderedProduct
                    {
                        OrderedProductId = Guid.NewGuid().ToString(),
                        OrderId = newOrderId,
                        ProductId = p.ProductId,
                        Price = products.First(pr => pr.ProductId == p.ProductId).Price,
                        Count = p.Count
                    }),
                    Status = order.Status,
                    AddDate = order.AddDate,
                    ModifiedDate = order.ModifiedDate
                };

                _shopContext.Orders.Add(orderEntity);
                await _shopContext.SaveChangesAsync();

                result.IsSuccess = true;
                result.Data = new OrderDTO
                {
                    OrderId = orderEntity.OrderId,
                    UserId = orderEntity.UserId,
                    OrderedProducts = orderEntity.OrderedProducts.Select(op => new OrderedProductDTO
                    {
                        OrderId = op.OrderId,
                        ProductId = op.ProductId,
                        Name = products.First(p => p.ProductId == op.ProductId).Name,
                        Price = op.Price,
                        Count = op.Count
                    }).ToList(),
                    Status = orderEntity.Status,
                    AddDate = orderEntity.AddDate,
                    ModifiedDate = orderEntity.ModifiedDate
                };
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = "CREATE_ORDER_ERROR";

                await _logger.LogResultAsync(result, nameof(CreateOrderAsync));
            }

            return result;
        }

        public async Task<OrderDTO> ReadOrderAsync(string userId, string orderId)
        {
            var order = await _shopContext.Orders
                .Where(o => o.UserId == userId && o.OrderId == orderId)
                .Select(order => new OrderDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderedProducts = order.OrderedProducts.Select(op => new OrderedProductDTO
                    {
                        OrderId = op.OrderId,
                        Name = op.Product.Name,
                        ProductId = op.ProductId,
                        Price = op.Price,
                        Count = op.Count
                    }).ToList(),
                    Status = order.Status,
                    AddDate = order.AddDate,
                    ModifiedDate = order.ModifiedDate
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<ServiceResultDTO<OrderDTO>> UpdateOrderAsync(string userId, OrderDTO orderDto)
        {
            var result = new ServiceResultDTO<OrderDTO>();
            try
            {
                var productIds = orderDto.OrderedProducts.Select(op => op.ProductId).ToList();
                var existingProducts = await _shopContext.Products.Where(op => productIds.Contains(op.ProductId)).ToListAsync();

                if (existingProducts.Count != productIds.Count)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "One or more products do not exist";
                    return result;
                }

                var existingOrder = await _shopContext.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == orderDto.OrderId && o.UserId == userId);

                if (existingOrder == null)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Order not found";
                    return result;
                }

                var existingOrderedProducts = await _shopContext.OrderedProducts
                    .Where(op => op.OrderId == orderDto.OrderId)
                    .ToListAsync();

                var orderedProducts = orderDto.OrderedProducts.Select(p => new OrderedProduct
                {
                    OrderId = orderDto.OrderId,
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList();

                var compareResult = _listComparer.Compare(existingOrderedProducts, orderedProducts, op => op.OrderId);
                // add new ordered products
                foreach (var added in compareResult.Added)
                {
                    added.OrderId = orderDto.OrderId;
                    added.Price = existingProducts.First(p => p.ProductId == added.ProductId).Price;
                }
                _shopContext.OrderedProducts.AddRange(compareResult.Added);
                // remove ordered products
                _shopContext.OrderedProducts.RemoveRange(compareResult.Removed);
                // update ordered products
                foreach (var modified in compareResult.Updated)
                {
                    var product = orderedProducts.First(op => op.ProductId == modified.ProductId);
                    modified.Count = product.Count;
                }
                // update order
                existingOrder.Status = OrderStatus.Updated;
                existingOrder.ModifiedDate = DateTime.UtcNow;

                await _shopContext.SaveChangesAsync();

                result.IsSuccess = true;
                result.Data = await ReadOrderAsync(userId, orderDto.OrderId);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = "UPDATE_ORDER_ERROR";
                await _logger.LogResultAsync(result, nameof(UpdateOrderAsync));
            }

            return result;
        }

        public async Task<ServiceResultDTO<string>> DeleteOrderAsync(string orderId)
        {
            var result = new ServiceResultDTO<string>();
            try
            {
                var order = await _shopContext.Orders.FindAsync(orderId);
                if (order != null)
                {
                    _shopContext.Orders.Remove(order);
                    await _shopContext.SaveChangesAsync();
                    result.IsSuccess = true;
                    result.Data = orderId;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = "Order not found";
                    result.Data = null;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = "DELETE_ORDER_ERROR";
                await _logger.LogResultAsync(result, nameof(DeleteOrderAsync));
            }

            return result;
        }

        public async Task<ServiceResultDTO<IEnumerable<OrderDTO>>> ReadUserOrdersAsync()
        {
            var result = new ServiceResultDTO<IEnumerable<OrderDTO>>();
            try
            {
                var orders = await _shopContext.Orders
                    .Select(order => new OrderDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        OrderedProducts = order.OrderedProducts.Select(op => new OrderedProductDTO
                        {
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

                result.IsSuccess = true;
                result.Data = orders;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = "READ_ORDERS_ERROR";
                await _logger.LogResultAsync(result, nameof(ReadUserOrdersAsync));
            }

            return result;
        }

        public async Task<ServiceResultDTO<IEnumerable<OrderDTO>>> ReadUserOrdersAsync(string userId)
        {
            var result = new ServiceResultDTO<IEnumerable<OrderDTO>>();
            try
            {
                var orders = await _shopContext.Orders
                    .Where(order => order.UserId == userId)
                    .Select(order => new OrderDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        OrderedProducts = order.OrderedProducts.Select(op => new OrderedProductDTO
                        {
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

                result.IsSuccess = true;
                result.Data = orders;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = "READ_ORDERS_ERROR";
                await _logger.LogResultAsync(result, nameof(ReadUserOrdersAsync));
            }

            return result;
        }
    }
}
