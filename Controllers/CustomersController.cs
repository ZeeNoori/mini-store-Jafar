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
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone
                })
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {

            var customer = await _customerContext.Customers.FindAsync(id);

            if (customer is null)
            {
                return NotFound("User does not exist");
            }

            var response = new CustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return Ok(response);
        }

        [HttpPost]

        public async Task<IActionResult> AddCustomer([FromBody] CustomerRequest request)
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

            response.Id = customer.Id;
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, response);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> EditCustomer([FromRoute] int id, [FromBody] CustomerRequest request)
        {
            var customer = await _customerContext.Customers.FindAsync(id);

            if (customer is null)
            {
                return NotFound();
            }


            var NewCustomer = new Customer
            {
                Id = id,
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone
            };

            customer.Id = NewCustomer.Id;
            customer.FullName = NewCustomer.FullName;
            customer.Email = NewCustomer.Email;
            customer.Phone = NewCustomer.Phone;

            _customerContext.Customers.Update(customer);

            await _customerContext.SaveChangesAsync();

            var response = new CustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone
            };
            return Ok(response);
        }


        [HttpDelete("{id:int}")]


        public async Task<IActionResult> RemoveCustomer([FromRoute] int id)
        {

            var customer = await _customerContext.Customers.FindAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            _customerContext.Customers.Remove(customer);
            await _customerContext.SaveChangesAsync();
            return NoContent();
        }

    }
}




