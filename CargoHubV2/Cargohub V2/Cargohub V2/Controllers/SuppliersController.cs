using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly SuppliersService _suppliersService;

        public SuppliersController(SuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public ActionResult<List<Supplier>> GetAllSuppliers()
        {
            var suppliers = _suppliersService.GetAllSuppliers();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public ActionResult<Supplier> GetSupplierById(int id)
        {
            var supplier = _suppliersService.GetSupplierById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpPost]
        public ActionResult<Supplier> AddSupplier([FromBody] Supplier supplier)
        {
            var newSupplier = _suppliersService.AddSupplier(supplier);
            if (newSupplier == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetSupplierById), new { id = newSupplier.Id }, newSupplier);
        }

        [HttpPut("{id}")]
        public ActionResult<Supplier> UpdateSupplier(int id, [FromBody] Supplier updatedSupplier)
        {
            var supplier = _suppliersService.UpdateSupplier(id, updatedSupplier);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSupplier(int id)
        {
            var supplierDeleted = _suppliersService.DeleteSupplier(id);
            if (!supplierDeleted)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("{id}")]
        public ActionResult AddProductPrice(int id, [FromQuery] int price)
        {
            var success = _suppliersService.AddProductPrice(id, price);
            if (!success)
            {
                var supplierExists = _suppliersService.GetSupplierById(id) != null;
                if (!supplierExists)
                {
                    return NotFound();
                }
                return BadRequest();
            }
            return Ok();
        }
    }
}
