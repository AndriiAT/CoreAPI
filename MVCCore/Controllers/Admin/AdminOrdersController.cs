using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Orders;
using Persistance.DTOs.Orders;
using Persistance.DTOs.Products;
using Persistance.Repositories;
using Persistance.Services;
using System.Linq;
using System.Threading.Tasks;
using static Persistance.DTOs.Orders.Enums;

namespace MVCCore.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    [ApiController]
    [Route("[controller]")]
    public class OrdersAdminController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly ICustomAuthorizationService _authorizationService;

        public OrdersAdminController(IOrderRepository ordersRepository, ICustomAuthorizationService authorizationService)
        {
            _ordersRepository = ordersRepository;
            _authorizationService = authorizationService;
        }

        // GET: api/orders
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var ordersResult = await _ordersRepository.ReadUserOrdersAsync();

            if (!ordersResult.IsSuccess)
            {
                return BadRequest(ordersResult.ErrorMessage);
            }

            return Ok(ordersResult.Data);
        }

        // POST: api/orders
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderBuildingModel order)
        {
            var user = await _authorizationService.GetAuthorizedUserAsync();
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
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string id, [FromBody] OrderBuildingModel order)
        {
            var user = await _authorizationService.GetAuthorizedUserAsync();
            var order_check = await _ordersRepository.ReadOrderAsync(user.UserId, id);

            if (order_check == null || order_check.UserId != user.UserId)
            {
                return NotFound();
            }

            var orderDTO = new OrderDTO
            {
                OrderId = id,
                UserId = user.UserId,
                OrderedProducts = order.Products.Select(p => new OrderedProductDTO
                {
                    OrderId = id,
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList(),
                ModifiedDate = System.DateTime.UtcNow
            };
            var updatedOrderResult = await _ordersRepository.UpdateOrderAsync(user.UserId, orderDTO);

            if (!updatedOrderResult.IsSuccess)
            {
                return BadRequest(updatedOrderResult.ErrorMessage);
            }
            return Ok(updatedOrderResult.Data);
        }

        // DELETE: api/orders
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string id)
        {
            var user = await _authorizationService.GetAuthorizedUserAsync();
            var order_check = await _ordersRepository.ReadOrderAsync(user.UserId, id);

            if (order_check == null || order_check.UserId != user.UserId)
            {
                return NotFound();
            }

            var deleteResult = await _ordersRepository.DeleteOrderAsync(id);
            if (!deleteResult.IsSuccess)
            {
                return BadRequest(deleteResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}
