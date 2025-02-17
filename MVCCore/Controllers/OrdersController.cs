﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Orders;
using Persistance.DTOs.Orders;
using Persistance.DTOs.Products;
using Persistance.Repositories;
using Persistance.Services;
using System.Linq;
using System.Threading.Tasks;
using static Persistance.DTOs.Orders.Enums;

namespace MVCCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrdersUserController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly ICustomAuthorizationService _authorizationService;

        public OrdersUserController(IOrderRepository ordersRepository, ICustomAuthorizationService authorizationService)
        {
            _ordersRepository = ordersRepository;
            _authorizationService = authorizationService;
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] string id)
        {
            var user = _authorizationService.GetAuthorizedUserAsync().Result;

            var order = await _ordersRepository.ReadOrderAsync(user.UserId, id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderBuildingModel order)
        {
            var user = _authorizationService.GetAuthorizedUserAsync().Result;
            if (user.UserId == null)
            {
                return BadRequest("You have no permission to create order");
            }

            var orderDTO = new OrderDTO
            {
                UserId = user.UserId,
                OrderedProducts = order.Products.Select(p => new OrderedProductDTO
                {
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList(),
                Status = OrderStatus.Placed,
                AddDate = System.DateTime.UtcNow,
                ModifiedDate = System.DateTime.UtcNow
            };

            var newOrderResult = await _ordersRepository.CreateOrderAsync(orderDTO);
            if (!newOrderResult.IsSuccess)
            {
                return BadRequest(newOrderResult.ErrorMessage);
            }
            return Ok(newOrderResult.Data);
        }

        // PUT: api/orders/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string id, [FromBody] OrderBuildingModel order)
        {
            var user = await _authorizationService.GetAuthorizedUserAsync();
            var userID = user.UserId;

            var order_check = await _ordersRepository.ReadOrderAsync(userID, id);

            if (order_check == null || (User.IsInRole("User") && order_check.UserId != userID))
            {
                return NotFound();
            }

            var orderDTO = new OrderDTO
            {
                OrderId = id,
                UserId = userID,
                OrderedProducts = order.Products.Select(p => new OrderedProductDTO
                {
                    OrderId = id,
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList(),
                ModifiedDate = System.DateTime.UtcNow
            };
            var updatedOrderResult = await _ordersRepository.UpdateOrderAsync(userID, orderDTO);

            if (!updatedOrderResult.IsSuccess)
            {
                return BadRequest(updatedOrderResult.ErrorMessage);
            }
            return Ok(updatedOrderResult.Data);
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var user = await _authorizationService.GetAuthorizedUserAsync();
            var order = await _ordersRepository.ReadOrderAsync(user.UserId, id.ToString());
            
            if (order == null)
            {
                return NotFound();
            }
            
            await _ordersRepository.DeleteOrderAsync(id.ToString());
            return NoContent();
        }
    }
}
