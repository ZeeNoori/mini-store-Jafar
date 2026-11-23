using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync() =>
            await _context.Customers.AsNoTracking().ToListAsync();

        public async Task<Customer?> GetCustomerByIdAsync(int id) =>
            await _context.Customers.FindAsync(id);

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> EditCustomerAsync(int id, Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer is null) return null;

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<bool> RemoveCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer is null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
