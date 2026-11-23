using System;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category?> EditCategoryAsync(int id, Category category);
        Task<bool> RemoveCategoryAsync(int id);
    }
}
