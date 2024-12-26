using Microsoft.AspNetCore.Mvc;
using Persistance.DTOs;
using Persistance.Repositories;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
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
            return Ok(await _productRepository.ReadAllAsync());
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.ReadAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO product)
        {
            var newProduct = await _productRepository.CreateAsync(product);
            return Ok(newProduct);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var updatedProduct = await _productRepository.UpdateAsync(product);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
