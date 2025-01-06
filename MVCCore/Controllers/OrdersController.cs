using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Orders;
using Persistance.DTOs;
using Persistance.Repositories;
using System.Linq;
using System.Threading.Tasks;
using static Persistance.DTOs.Enums;

namespace MVCCore.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _ordersRepository;

        public OrdersController(IOrderRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersRepository.ReadAllAsync();
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            var order = await _ordersRepository.ReadAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost("Create/{userID}")]
        public async Task<IActionResult> CreateOrder([FromRoute] string userID, [FromBody] OrderBuildingModel order)
        {
            var orderDTO = new OrderDTO
            {
                UserId = userID,
                OrderedProducts = order.Products.Select(p => new OrderedProductDTO
                {
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList(),
                Status = OrderStatus.Placed,
                AddDate = System.DateTime.UtcNow,
                ModifiedDate = System.DateTime.UtcNow
            };

            var newOrderResult = await _ordersRepository.CreateAsync(orderDTO);
            if (!newOrderResult.IsSuccess)
            {
                return BadRequest(newOrderResult.ErrorMessage);
            }
            return Ok(newOrderResult.Data);
        }

        // PUT: api/orders/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderBuildingModel order)
        {
            var orderDTO = new OrderDTO
            {
                Id = id,
                OrderedProducts = order.Products.Select(p => new OrderedProductDTO
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Count = p.Count,
                }).ToList(),
            };
            var updatedOrderResult = await _ordersRepository.UpdateAsync(orderDTO);

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
            await _ordersRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
