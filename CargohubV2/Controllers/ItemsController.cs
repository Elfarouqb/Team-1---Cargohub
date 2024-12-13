using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        // GET: api/Items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound(new { Message = $"Item with ID {id} not found." });
            }

            return Ok(item);
        }

        // GET: api/Items/ByItemLine/{itemLineId}
        [HttpGet("ByItemLine/{itemLineId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByItemLine(int itemLineId)
        {
            var items = await _itemService.GetItemsByItemLineAsync(itemLineId);
            return Ok(items);
        }

        // GET: api/Items/ByItemGroup/{itemGroupId}
        [HttpGet("ByItemGroup/{itemGroupId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByItemGroup(int itemGroupId)
        {
            var items = await _itemService.GetItemsByItemGroupAsync(itemGroupId);
            return Ok(items);
        }

        // GET: api/Items/ByItemType/{itemTypeId}
        [HttpGet("ByItemType/{itemTypeId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByItemType(int itemTypeId)
        {
            var items = await _itemService.GetItemsByItemTypeAsync(itemTypeId);
            return Ok(items);
        }

        // GET: api/Items/BySupplier/{supplierId}
        [HttpGet("BySupplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsBySupplier(int supplierId)
        {
            var items = await _itemService.GetItemsBySupplierAsync(supplierId);
            return Ok(items);
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> AddItem([FromBody] Item newItem)
        {
            var createdItem = await _itemService.AddItemAsync(newItem);
            return CreatedAtAction(nameof(GetItemById), new { id = createdItem.Id }, createdItem);
        }

        // PUT: api/Items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item updatedItem)
        {
            if (id != updatedItem.Id)
            {
                return BadRequest(new { Message = "ID in the URL does not match the ID in the payload." });
            }

            var success = await _itemService.UpdateItemAsync(id, updatedItem);

            if (!success)
            {
                return NotFound(new { Message = $"Item with ID {id} not found." });
            }

            return NoContent();
        }

        // DELETE: api/Items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var success = await _itemService.RemoveItemAsync(id);

            if (!success)
            {
                return NotFound(new { Message = $"Item with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
