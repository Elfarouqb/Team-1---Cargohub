using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : Controller
    {
        private readonly SuppliersService _suppliersService;

        public SuppliersController(SuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            var suppliers = await _suppliersService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        // GET: api/Suppliers/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult<Supplier>> GetSupplierByCode(string code)
        {
            var supplier = await _suppliersService.GetSupplierByCodeAsync(code);
            if (supplier == null)
            {
                return NoContent();
            }
            return Ok(supplier);
        }

        // POST: api/Suppliers/Add
        [HttpPost("Add")]
        public async Task<IActionResult> CreateSupplier([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSupplier = await _suppliersService.GetSupplierByCodeAsync(supplier.Code);
            if (existingSupplier != null)
            {
                return BadRequest("Supplier with the same code already exists.");
            }

            var createdSupplier = await _suppliersService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplierByCode), new { code = createdSupplier.Code }, createdSupplier);
        }

        // PUT: api/Suppliers/Update/{code}
        [HttpPut("Update/{code}")]
        public async Task<IActionResult> UpdateSupplier(string code, [FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSupplier = await _suppliersService.UpdateSupplierAsync(supplier, code);
            if (updatedSupplier == null)
            {
                return NoContent();
            }

            return Ok(updatedSupplier);
        }

        // DELETE: api/Suppliers/Delete/{code}
        [HttpDelete("Delete/{code}")]
        public async Task<IActionResult> RemoveSupplierByCode(string code)
        {
            var supplier = await _suppliersService.RemoveSupplierByCodeAsync(code);
            if (supplier == null)
            {
                return NoContent();
            }

            return Ok(supplier);
        }
    }
}
