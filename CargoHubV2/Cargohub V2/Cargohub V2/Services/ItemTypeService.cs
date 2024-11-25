using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class ItemTypeService
    {
        private readonly CargoHubDbContext _context;

        public ItemTypeService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item_Type>> GetAllItemTypesAsync()
        {
            return await _context.Items_Types.ToListAsync();
        }

        public async Task<Item_Type> GetItemTypeByIdAsync(int id)
        {
            return await _context.Items_Types.FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<Item_Type> AddItemTypeAsync(Item_Type newItemType)
        {
            _context.Items_Types.Add(newItemType);
            await _context.SaveChangesAsync();
            return newItemType;
        }

        public async Task<bool> UpdateItemTypeAsync(int id, Item_Type updatedItemType)
        {
            var existingItemType = await _context.Items_Types.FindAsync(id);

            if (existingItemType == null)
            {
                return false;
            }

            existingItemType.Name = updatedItemType.Name;
            existingItemType.Description = updatedItemType.Description;
            existingItemType.UpdatedAt = DateTime.UtcNow; // Update timestamp

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItemTypeAsync(int id)
        {
            var itemType = await _context.Items_Types.FindAsync(id);

            if (itemType == null)
            {
                return false;
            }

            _context.Items_Types.Remove(itemType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
