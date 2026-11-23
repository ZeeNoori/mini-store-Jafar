using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetAllOrdersAsync() =>
            await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .ToListAsync();

        public async Task<Order?> GetOrderByIdAsync(int id) =>
            await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .SingleOrDefaultAsync(order => order.Id == id);

        public async Task<bool> CustomerExistsAsync(int customerId) =>
            await _context.Customers.AnyAsync(customer => customer.Id == customerId);

        public async Task<Product?> GetProductByIdAsync(int productId) =>
            await _context.Products.AsNoTracking().SingleOrDefaultAsync(product => product.Id == productId);

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> EditOrderAsync(int id, Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder is null) return null;

            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Status = order.Status;
            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> RemoveOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
