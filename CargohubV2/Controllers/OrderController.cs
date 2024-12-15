//ordercontroller
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cargohub_V2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("{orderId}/items")]
        public async Task<ActionResult<List<OrderStock>>> GetItemsInOrder(int orderId)
        {
            var items = await _orderService.GetItemsInOrderAsync(orderId);
            return Ok(items);
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForShipment(int shipmentId)
        {
            var orders = await _orderService.GetOrdersForShipmentAsync(shipmentId);
            return Ok(orders);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForClient(string clientId)
        {
            var orders = await _orderService.GetOrdersForClientAsync(clientId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order newOrder)
        {
            var createdOrder = await _orderService.AddOrderAsync(newOrder);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] Order updatedOrder)
        {
            var success = await _orderService.UpdateOrderAsync(orderId, updatedOrder);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var success = await _orderService.DeleteOrderAsync(orderId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
