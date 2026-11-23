using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> CustomerExistsAsync(int customerId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Order> AddOrderAsync(Order order);
        Task<Order?> EditOrderAsync(int id, Order order);
        Task<bool> RemoveOrderAsync(int id);
    }
}
