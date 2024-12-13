using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypesController : Controller
    {
        private readonly ItemTypeService _itemTypeService;

        public ItemTypesController(ItemTypeService itemTypeService)
        {
            _itemTypeService = itemTypeService;
        }

        // GET: api/ItemTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item_Type>>> GetAllItemTypes()
        {
            var itemTypes = await _itemTypeService.GetAllItemTypesAsync();
            return Ok(itemTypes);
        }

        // GET: api/ItemTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item_Type>> GetItemTypeById(int id)
        {
            var itemType = await _itemTypeService.GetItemTypeByIdAsync(id);

            if (itemType == null)
            {
                return NotFound(new { Message = $"Item type with ID {id} not found." });
            }

            return Ok(itemType);
        }

        // POST: api/ItemTypes
        [HttpPost]
        public async Task<ActionResult<Item_Type>> AddItemType([FromBody] Item_Type newItemType)
        {
            var createdItemType = await _itemTypeService.AddItemTypeAsync(newItemType);
            return CreatedAtAction(nameof(GetItemTypeById), new { id = createdItemType.Id }, createdItemType);
        }

        // PUT: api/ItemTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemType(int id, [FromBody] Item_Type updatedItemType)
        {
            if (id != updatedItemType.Id)
            {
                return BadRequest(new { Message = "ID in the URL does not match the ID in the payload." });
            }

            var success = await _itemTypeService.UpdateItemTypeAsync(id, updatedItemType);

            if (!success)
            {
                return NotFound(new { Message = $"Item type with ID {id} not found." });
            }

            return NoContent();
        }

        // DELETE: api/ItemTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemType(int id)
        {
            var success = await _itemTypeService.DeleteItemTypeAsync(id);

            if (!success)
            {
                return NotFound(new { Message = $"Item type with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
