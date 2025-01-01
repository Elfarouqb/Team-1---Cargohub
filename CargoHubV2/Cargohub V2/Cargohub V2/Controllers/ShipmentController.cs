
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Cargohub_V2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentService _shipmentService;

        public ShipmentsController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Shipment>>> GetAllShipments()
        {
            var shipments = await _shipmentService.GetAllShipmentsAsync();
            return Ok(shipments);
        }

        [HttpGet("{shipmentId}")]
        public async Task<ActionResult<Shipment>> GetShipmentById(int shipmentId)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }

        [HttpGet("{shipmentId}/items")]
        public async Task<ActionResult<List<ShipmentItem>>> GetItemsInShipment(int shipmentId)
        {
            var items = await _shipmentService.GetItemsInShipmentAsync(shipmentId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
        {
            if (shipment == null)
            {
                return BadRequest("Shipment cannot be null.");
            }

            // Call the service to add the shipment
            var createdShipment = await _shipmentService.AddShipmentAsync(shipment);

            // Check if the shipment and its items were added correctly
            if (createdShipment == null || createdShipment.Id <= 0)
            {
                return StatusCode(500, "Failed to create the shipment.");
            }

            // Return a CreatedAtAction response with a route to GetShipmentById
            return CreatedAtAction(nameof(GetShipmentById), new { id = createdShipment.Id }, createdShipment);
        }





        [HttpPut("{shipmentId}")]
        public async Task<IActionResult> UpdateShipment(int shipmentId, [FromBody] Shipment updatedShipment)
        {
            var success = await _shipmentService.UpdateShipmentAsync(shipmentId, updatedShipment);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{shipmentId}/items")]
        public async Task<IActionResult> UpdateItemsInShipment(int shipmentId, [FromBody] List<ShipmentItem> updatedItems)
        {
            var success = await _shipmentService.UpdateItemsInShipmentAsync(shipmentId, updatedItems);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{shipmentId}")]
        public async Task<IActionResult> RemoveShipment(int shipmentId)
        {
            var success = await _shipmentService.RemoveShipmentAsync(shipmentId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
