using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;

namespace Cargohub_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ClientsService _clientsService;

        public ClientsController(ClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientsService.GetAllClientsAsync();
            return Ok(clients);
        }

        // GET: api/Clients/{id}
        [HttpGet("{id}")] // Route parameter
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientsService.GetClientByIdAsync(id);
            return Ok(client);
        }

        // POST: api/Clients/AddClient
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return validation errors
            }

            var createdClient = await _clientsService.CreateClientAsync(client);
            if (createdClient == null)
            {
                return Conflict("Client already exists.");
            }

            return CreatedAtAction(nameof(GetClientById), new { id = createdClient.Id }, createdClient);
        }

    }
}
