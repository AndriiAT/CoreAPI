using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Products;
using Persistance.DTOs;
using Persistance.Repositories;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    [Authorize]
    [ApiController]
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
            var products = await _productRepository.ReadAllAsync();
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);

        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
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
        public async Task<IActionResult> CreateProduct([FromBody] ProductBuildingModel product)
        {
            var productDTO = new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };

            var newProduct = await _productRepository.CreateAsync(productDTO);
            return Ok(newProduct);
        }

        // PUT: api/products/5
        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The product data to update.</param>
        /// <returns>The updated product.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductBuildingModel product)
        {
            var existingProduct = await _productRepository.ReadAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var productDTO = new ProductDTO
            {
                ProductId = id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };

            var updatedProduct = await _productRepository.UpdateAsync(productDTO);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
