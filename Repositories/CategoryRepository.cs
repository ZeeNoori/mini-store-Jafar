using System;
using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _categoryContext;

        public CategoryRepository(AppDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _categoryContext.Categories.AddAsync(category);

            await _categoryContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> EditCategoryAsync(int id, Category category)
        {
            var existingCategory = await _categoryContext.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _categoryContext.SaveChangesAsync();
            return existingCategory;

        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {

            var categories = await _categoryContext.Categories.AsNoTracking()
                .ToListAsync();

            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {

            return await _categoryContext.Categories.FindAsync(id);

        }

        public async Task<bool> RemoveCategoryAsync(int id)
        {
            var category = await _categoryContext.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return false;
            }

            _categoryContext.Remove(category);
            await _categoryContext.SaveChangesAsync();

            return true;

        }
    }
}
