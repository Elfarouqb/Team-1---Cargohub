using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemLinesController : Controller
    {
        private readonly ItemLineService _itemLineService;

        public ItemLinesController(ItemLineService itemLineService)
        {
            _itemLineService = itemLineService;
        }

        // GET: api/ItemLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item_Line>>> GetAllItemLines()
        {
            var itemLines = await _itemLineService.GetAllItemLinesAsync();
            return Ok(itemLines);
        }

        // GET: api/ItemLines/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item_Line>> GetItemLineById(int id)
        {
            var itemLine = await _itemLineService.GetItemLineByIdAsync(id);

            if (itemLine == null)
            {
                return NotFound(new { Message = $"Item line with ID {id} not found." });
            }

            return Ok(itemLine);
        }

        // POST: api/ItemLines
        [HttpPost]
        public async Task<ActionResult<Item_Line>> AddItemLine([FromBody] Item_Line newItemLine)
        {
            var createdItemLine = await _itemLineService.AddItemLineAsync(newItemLine);
            return CreatedAtAction(nameof(GetItemLineById), new { id = createdItemLine.Id }, createdItemLine);
        }

        // PUT: api/ItemLines/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemLine(int id, [FromBody] Item_Line updatedItemLine)
        {
            if (id != updatedItemLine.Id)
            {
                return BadRequest(new { Message = "ID in the URL does not match the ID in the payload." });
            }

            var success = await _itemLineService.UpdateItemLineAsync(id, updatedItemLine);

            if (!success)
            {
                return NotFound(new { Message = $"Item line with ID {id} not found." });
            }

            return NoContent();
        }

        // DELETE: api/ItemLines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemLine(int id)
        {
            var success = await _itemLineService.DeleteItemLineAsync(id);

            if (!success)
            {
                return NotFound(new { Message = $"Item line with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
