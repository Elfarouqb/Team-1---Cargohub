using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemGroupsController : Controller
    {
        private readonly ItemGroupService _itemGroupService;

        public ItemGroupsController(ItemGroupService itemGroupService)
        {
            _itemGroupService = itemGroupService;
        }

        // GET: api/ItemGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item_Group>>> GetAllItemGroups()
        {
            var itemGroups = await _itemGroupService.GetAllItemGroupsAsync();
            return Ok(itemGroups);
        }

        // GET: api/ItemGroups/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item_Group>> GetItemGroupById(int id)
        {
            var itemGroup = await _itemGroupService.GetItemGroupByIdAsync(id);

            if (itemGroup == null)
            {
                return NotFound(new { Message = $"Item group with ID {id} not found." });
            }

            return Ok(itemGroup);
        }

        // POST: api/ItemGroups
        [HttpPost]
        public async Task<ActionResult<Item_Group>> AddItemGroup([FromBody] Item_Group itemGroup)
        {
            var createdItemGroup = await _itemGroupService.AddItemGroupAsync(itemGroup);
            return CreatedAtAction(nameof(GetItemGroupById), new { id = createdItemGroup.Id }, createdItemGroup);
        }

        // DELETE: api/ItemGroups/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteItemGroup(int id)
        {
            var success = await _itemGroupService.RemoveItemGroupAsync(id);

            if (!success)
            {
                return NotFound(new { Message = $"Item group with ID {id} not found." });
            }

            return NoContent();
        }

        // PUT: api/ItemGroups/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemGroup(int id, [FromBody] Item_Group updatedItemGroup)
        {
            if (id != updatedItemGroup.Id)
            {
                return BadRequest(new { Message = "ID in the URL does not match the ID in the payload." });
            }

            var success = await _itemGroupService.UpdateItemGroupAsync(id, updatedItemGroup);

            if (!success)
            {
                return NotFound(new { Message = $"Item group with ID {id} not found." });
            }

            return NoContent(); // Return 204 No Content if the update is successful
        }
    }
}
