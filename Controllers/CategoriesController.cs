using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Models;
using MiniStoreManagement.Repositories;

namespace MiniStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]


        public async Task<IActionResult> GetAllCategories()
        {

            var categories = await _categoryRepository.GetAllCategories();

            var listed = categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });


            var result = new ApiResponse<CategoryResponse>
            {
                Items = listed
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

            var createdCategory = await _categoryRepository.AddCategoryAsync(category);

            var response = new CategoryResponse
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, response);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {

            var category = await _categoryRepository.GetCategoryByIdAsync(id);

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
            var newCategory = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            var modifiedCategory = await _categoryRepository.EditCategoryAsync(id, newCategory);

            if (modifiedCategory is null)
            {
                return NotFound();
            }
            var result = new CategoryResponse
            {

                Id = id,
                Name = modifiedCategory.Name,
                Description = modifiedCategory.Description

            };
            return Ok(result);
        }


        [HttpDelete("{id:int}")]

        public async Task<IActionResult> RemoveCategory([FromRoute] int id)
        {

            var deleted = await _categoryRepository.RemoveCategoryAsync(id);

            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}



















