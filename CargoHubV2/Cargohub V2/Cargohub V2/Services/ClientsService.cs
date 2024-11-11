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

        public async Task<Client> CreateClientAsync(Client client)
        {
            DateTime CreatedAt = DateTime.UtcNow;
            DateTime UpdatedAt = DateTime.UtcNow;

            client.CreatedAt = new DateTime(CreatedAt.Year, CreatedAt.Month, CreatedAt.Day, CreatedAt.Hour, CreatedAt.Minute, CreatedAt.Second, DateTimeKind.Utc);
            client.UpdatedAt = new DateTime(UpdatedAt.Year, UpdatedAt.Month, UpdatedAt.Day, UpdatedAt.Hour, UpdatedAt.Minute, UpdatedAt.Second, DateTimeKind.Utc);

            // Add the new client
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }
    }
}
