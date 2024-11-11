using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class ItemLineService
    {
        private readonly CargoHubDbContext _context;

        // Constructor to inject CargoHubDbContext
        public ItemLineService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item_Line>> GetAllItemLinesAsync()
        {
            return await _context.Items_Lines.ToListAsync();
        }

        public async Task<Item_Line> GetItemLineByIdAsync(int id)
        {
            return await _context.Items_Lines.FirstOrDefaultAsync(ig => ig.Id == id);
        }
    }
}
