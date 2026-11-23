using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _context.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _context.Products.FindAsync(id);

        public async Task<bool> CategoryExistsAsync(int categoryId) =>
            await _context.Categories.AnyAsync(category => category.Id == categoryId);

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> EditProductAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct is null) return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> RemoveProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
