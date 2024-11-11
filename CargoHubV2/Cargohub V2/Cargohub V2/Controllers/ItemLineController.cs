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
                return NotFound($"Item line with ID {id} not found.");
            }

            return Ok(itemLine);
        }
    }
}
