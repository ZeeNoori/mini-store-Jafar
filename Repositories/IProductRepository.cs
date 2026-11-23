using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<Product> AddProductAsync(Product product);
        Task<Product?> EditProductAsync(int id, Product product);
        Task<bool> RemoveProductAsync(int id);
    }
}
