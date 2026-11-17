using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Data;
using MiniStoreManagement.Models;

namespace MiniStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly AppDbContext _customerContext;
        public CustomersController(AppDbContext context)
        {
            _customerContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerContext.Customers.ToListAsync();

            var response = new ApiResponse<CustomerResponse>
            {
                Items = customers.Select(c => new CustomerResponse
                {
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone
                })
            };

            return Ok(response);
        }

        [HttpPost]

        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerRequest request)
        {


            var customer = new Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone
            };

            await _customerContext.Customers.AddAsync(customer);

            await _customerContext.SaveChangesAsync();

            var response = new CustomerResponse
            {
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return Ok(response);
        }
    }
}



