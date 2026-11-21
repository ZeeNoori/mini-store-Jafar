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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(order => order.OrderItems)
                .ToListAsync();

            var response = new ApiResponse<OrderResponse>
            {
                Items = orders.Select(order => new OrderResponse
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Items = order.OrderItems.Select(item => new OrderItemResponse
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                })
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(order => order.OrderItems)
                .SingleOrDefaultAsync(order => order.Id == id);

            if (order is null)
            {
                return NotFound();
            }

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.OrderItems.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer is null)
            {
                return BadRequest("Customer does not exist.");
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                Status = request.Status
            };

            foreach (var requestedProduct in request.Items)
            {
                var product = await _context.Products.FindAsync(requestedProduct.ProductId);

                if (product is null)
                {
                    return BadRequest("Product does not exist.");
                }

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = requestedProduct.Quantity,
                    UnitPrice = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.OrderItems.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
            };

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditOrder(
            [FromRoute] int id,
            [FromBody] OrderUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer is null)
            {
                return BadRequest("Customer does not exist.");
            }

            order.CustomerId = request.CustomerId;
            order.OrderDate = request.OrderDate;
            order.Status = request.Status;

            await _context.SaveChangesAsync();

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveOrder([FromRoute] int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
