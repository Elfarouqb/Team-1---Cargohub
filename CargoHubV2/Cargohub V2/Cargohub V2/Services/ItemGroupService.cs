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

        public async Task<Item_Group> AddItemGroupAsync(Item_Group itemGroup)
        {
            _context.Items_Groups.Add(itemGroup);
            await _context.SaveChangesAsync();
            return itemGroup;
        }

        public async Task<bool> RemoveItemGroupAsync(int id)
        {
            var itemGroup = await _context.Items_Groups.FindAsync(id);

            if (itemGroup == null)
                return false;

            _context.Items_Groups.Remove(itemGroup);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateItemGroupAsync(int id, Item_Group updatedItemGroup)
        {
            var existingItemGroup = await _context.Items_Groups.FindAsync(id);

            if (existingItemGroup == null)
            {
                return false; // Item group not found
            }

            // Update fields from the payload
            existingItemGroup.Name = updatedItemGroup.Name;
            existingItemGroup.Description = updatedItemGroup.Description;
            existingItemGroup.UpdatedAt = DateTime.UtcNow; // Set the updated_at field to current time

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
