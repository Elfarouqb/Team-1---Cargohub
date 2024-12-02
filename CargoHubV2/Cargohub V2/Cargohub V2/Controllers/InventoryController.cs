using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;



namespace Cargohub_V2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;
        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public ActionResult<List<Inventory>> GetAllInventories()
        {
            var inventories = _inventoryService.GetAllInventories();
            return Ok(inventories);
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<Inventory> GetInventoryById(int id)
        {
            var inventory = _inventoryService.GetInventoryByID(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpGet("item/{itemId}")]
        public ActionResult<List<Inventory>> GetInventoriesForItem(string itemId)
        {
            var inventories = _inventoryService.GetInventoriesForItem(itemId);
            if (inventories == null || inventories.Count == 0)
            {
                return NotFound();
            }
            return Ok(inventories);
        }

        [HttpPost]
        [Route("Post")]
        public ActionResult<Inventory> AddInventory([FromBody] Inventory inventory)
        {

            var newInventory = _inventoryService.AddInventory(inventory);

            if (newInventory == null)
            {
                return BadRequest("Inventory data is null or id already exists");
            }

            return Created("", newInventory);
        }

        [HttpPut("location/Stock/{id}")]
        public IActionResult UpdateLocationStock(int id)
        {
            try
            {
                _inventoryService.UpdateLocationStock(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Inventory> UpdateInventory(int id, [FromBody] Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Inventory data is null");
            }

            var updatedInventory = _inventoryService.UpdateInventory(id, inventory);
            if (updatedInventory == null)
            {
                return NotFound();
            }

            return Ok(updatedInventory);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInventory(int id)
        {
            var deleted = _inventoryService.DeleteInventory(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }


    }

}