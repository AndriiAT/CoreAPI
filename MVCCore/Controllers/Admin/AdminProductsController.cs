using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCCore.Models.Products;
using Persistance.DTOs.Products;
using Persistance.Repositories.Products;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers.Admin
{
    [Authorize(Roles = "Admin,Manager")]
    [ApiController]
    [Route("[controller]")]
    public class ProductsAdminController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsAdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // POST: products
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBuildingModel product)
        {
            var productDTO = new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };

            var newProduct = await _productRepository.CreateProductAsync(productDTO);
            return Ok(newProduct);
        }

        // PUT: products
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromBody] ProductBuildingModel product)
        {
            var existingProduct = await _productRepository.ReadProductAsync(id.ToString());
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

            var updatedProduct = await _productRepository.UpdateProductAsync(productDTO);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        // DELETE: products
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            await _productRepository.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
