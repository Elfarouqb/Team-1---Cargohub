using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class ItemGroupService
    {
        private readonly CargoHubDbContext _context;

        // Constructor to inject CargoHubDbContext
        public ItemGroupService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item_Group>> GetAllItemGroupsAsync()
        {
            return await _context.Items_Groups.ToListAsync();
        }

        public async Task<Item_Group> GetItemGroupByIdAsync(int id)
        {
            return await _context.Items_Groups.FirstOrDefaultAsync(ig => ig.Id == id);
        }

    }
}
