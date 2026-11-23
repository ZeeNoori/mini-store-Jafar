using Microsoft.AspNetCore.Mvc;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Models;
using MiniStoreManagement.Repositories;

namespace MiniStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository) => _productRepository = productRepository;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();

            var response = new ApiResponse<ProductResponse>
            {
                Items = products.Select(product => new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId
                })
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
                return NotFound();

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> AddProduct(ProductRequest request)
        {
            if (!await _productRepository.CategoryExistsAsync(request.CategoryId))
                return BadRequest("Category does not exist.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };

            product = await _productRepository.AddProductAsync(product);

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponse>> EditProduct(int id, ProductRequest request)
        {
            if (await _productRepository.GetProductByIdAsync(id) is null)
                return NotFound();

            if (!await _productRepository.CategoryExistsAsync(request.CategoryId))
                return BadRequest("Category does not exist.");

            var product = await _productRepository.EditProductAsync(id, new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            });
            if (product is null) return NotFound();

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            if (!await _productRepository.RemoveProductAsync(id)) return NotFound();
            return NoContent();
        }
    }
}
