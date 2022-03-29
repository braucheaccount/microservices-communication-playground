using Microsoft.AspNetCore.Mvc;
using ProductService.Contracts.v1.Requests;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var data = await _productRepository.ListProductsAsync();

            return Ok(data);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductAsync([FromRoute] Guid id)
        {
            var data = await _productRepository.FindProductByIdAsync(id);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest createProductRequest)
        {
            var newProduct = new Product
            {
                ImageUrl = createProductRequest.ImageUrl,
                Name = createProductRequest.Name,
                Price = createProductRequest.Price,
                Quantity = createProductRequest.Quantity,
                Description = createProductRequest.Description,
            };

            var data = await _productRepository.CreateProductAsync(newProduct);

            return Ok(data);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid id)
        {
            await _productRepository.DeleteProductAsync(id);

            return NoContent();
        }
    }
}
