using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {


        private readonly AppDbContext _categoryContext;

        public CategoriesController(AppDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }


        [HttpGet]


        public async Task<IActionResult> GetAllCategories()
        {

            var categories = await _categoryContext.Categories
                .Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                })
                .ToListAsync();

            var result = new ApiResponse<CategoryResponse>
            {
                Items = categories
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            await _categoryContext.Categories.AddAsync(category);

            await _categoryContext.SaveChangesAsync();

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {

            var category = await _categoryContext.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            var result = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Ok(result);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> EditCategory([FromRoute] int id, [FromBody] CategoryRequest request)
        {

            var category = await _categoryContext.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            var newCategory = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            category.Name = newCategory.Name;
            category.Description = newCategory.Description;


            var modifiedCategory = _categoryContext.Categories.Update(category);

            await _categoryContext.SaveChangesAsync();

            var result = new CategoryResponse
            {

                Id = category.Id,
                Name = category.Name,
                Description = category.Description

            };
            return Ok(result);
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> RemoveCategory([FromRoute] int id)
        {
            var category = await _categoryContext.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            _categoryContext.Remove(category);
            await _categoryContext.SaveChangesAsync();
            return NoContent();
        }




    }
}



















