using Microsoft.AspNetCore.Mvc;
using MiniStoreManagement.Contracts.Requests;
using MiniStoreManagement.Contracts.Responses;
using MiniStoreManagement.Models;
using MiniStoreManagement.Repositories;

namespace MiniStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

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
            var order = await _orderRepository.GetOrderByIdAsync(id);

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
            if (!await _orderRepository.CustomerExistsAsync(request.CustomerId))
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
                var product = await _orderRepository.GetProductByIdAsync(requestedProduct.ProductId);

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

            order = await _orderRepository.AddOrderAsync(order);

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
            if (await _orderRepository.GetOrderByIdAsync(id) is null)
            {
                return NotFound();
            }

            if (!await _orderRepository.CustomerExistsAsync(request.CustomerId))
            {
                return BadRequest("Customer does not exist.");
            }

            var order = await _orderRepository.EditOrderAsync(id, new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                Status = request.Status
            });
            if (order is null) return NotFound();

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
            if (!await _orderRepository.RemoveOrderAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
