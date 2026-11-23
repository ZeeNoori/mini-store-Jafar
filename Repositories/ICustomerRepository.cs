using MiniStoreManagement.Models;

namespace MiniStoreManagement.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer?> EditCustomerAsync(int id, Customer customer);
        Task<bool> RemoveCustomerAsync(int id);
    }
}
