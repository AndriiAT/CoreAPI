using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Repositories.Products;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.ReadAllProductsAsync();
            if (!products.IsSuccess)
            {
                return NotFound($"error_Code: '{products.ErrorCode}', error_message: '{products.ErrorMessage}'");
            }

            return Ok(products.Data);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] string id)
        {
            var product = await _productRepository.ReadProductAsync(id.ToString());
            if (product.Data == null)
            {
                return NotFound($"Product with Id: {id} not found");
            }

            return Ok(product.Data);
        }
    }
}
