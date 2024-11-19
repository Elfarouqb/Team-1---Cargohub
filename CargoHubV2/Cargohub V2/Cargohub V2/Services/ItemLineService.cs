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
            return await _context.Items_Lines.FirstOrDefaultAsync(il => il.Id == id);
        }

        public async Task<Item_Line> AddItemLineAsync(Item_Line newItemLine)
        {
            _context.Items_Lines.Add(newItemLine);
            await _context.SaveChangesAsync();
            return newItemLine;
        }

        public async Task<bool> UpdateItemLineAsync(int id, Item_Line updatedItemLine)
        {
            var existingItemLine = await _context.Items_Lines.FindAsync(id);

            if (existingItemLine == null)
            {
                return false;
            }

            existingItemLine.Name = updatedItemLine.Name;
            existingItemLine.Description = updatedItemLine.Description;
            existingItemLine.UpdatedAt = DateTime.UtcNow; // Update timestamp

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemLineAsync(int id)
        {
            var itemLine = await _context.Items_Lines.FindAsync(id);

            if (itemLine == null)
            {
                return false;
            }

            _context.Items_Lines.Remove(itemLine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
