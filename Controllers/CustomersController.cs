using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Models;
using MiniStoreManagement.Repositories;

namespace MiniStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();

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

            var customer = await _customerRepository.GetCustomerByIdAsync(id);

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

            customer = await _customerRepository.AddCustomerAsync(customer);

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
            var newCustomer = new Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone
            };
            var customer = await _customerRepository.EditCustomerAsync(id, newCustomer);
            if (customer is null) return NotFound();

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

            if (!await _customerRepository.RemoveCustomerAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}




