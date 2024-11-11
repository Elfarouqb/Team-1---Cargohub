using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class ClientsService
    {
        private readonly CargoHubDbContext _context;

        // Constructor to inject CargoHubDbContext
        public ClientsService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            var possibleClient = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (possibleClient != null)
            {
                return possibleClient;
            }
            return null;
        }
    }
}
