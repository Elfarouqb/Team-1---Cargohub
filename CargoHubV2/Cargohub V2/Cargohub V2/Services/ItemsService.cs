using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class ItemService
    {
        private readonly CargoHubDbContext _context;

        public ItemService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _context.Items
                .Include(i => i.ItemLine)
                .Include(i => i.ItemGroup)
                .Include(i => i.ItemType)
                .Include(i => i.Supplier)
                .Take(50)
                .ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.ItemLine)
                .Include(i => i.ItemGroup)
                .Include(i => i.ItemType)
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Item>> GetItemsByItemLineAsync(int itemLineId)
        {
            return await _context.Items
                .Where(i => i.ItemLineId == itemLineId)
                .Include(i => i.ItemLine)
                .ToListAsync();
        }

        public async Task<List<Item>> GetItemsByItemGroupAsync(int itemGroupId)
        {
            return await _context.Items
                .Where(i => i.ItemGroupId == itemGroupId)
                .Include(i => i.ItemGroup)
                .ToListAsync();
        }

        public async Task<List<Item>> GetItemsByItemTypeAsync(int itemTypeId)
        {
            return await _context.Items
                .Where(i => i.ItemTypeId == itemTypeId)
                .Include(i => i.ItemType)
                .ToListAsync();
        }

        public async Task<List<Item>> GetItemsBySupplierAsync(int supplierId)
        {
            return await _context.Items
                .Where(i => i.SupplierId == supplierId)
                .Include(i => i.Supplier)
                .ToListAsync();
        }

        public async Task<Item> AddItemAsync(Item newItem)
        {
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }

        public async Task<bool> UpdateItemAsync(int id, Item updatedItem)
        {
            var existingItem = await _context.Items.FindAsync(id);

            if (existingItem == null)
            {
                return false;
            }

            existingItem.UId = updatedItem.UId;
            existingItem.Code = updatedItem.Code;
            existingItem.Description = updatedItem.Description;
            existingItem.ShortDescription = updatedItem.ShortDescription;
            existingItem.UpcCode = updatedItem.UpcCode;
            existingItem.ModelNumber = updatedItem.ModelNumber;
            existingItem.CommodityCode = updatedItem.CommodityCode;
            existingItem.ItemLineId = updatedItem.ItemLineId;
            existingItem.ItemGroupId = updatedItem.ItemGroupId;
            existingItem.ItemTypeId = updatedItem.ItemTypeId;
            existingItem.UnitPurchaseQuantity = updatedItem.UnitPurchaseQuantity;
            existingItem.UnitOrderQuantity = updatedItem.UnitOrderQuantity;
            existingItem.PackOrderQuantity = updatedItem.PackOrderQuantity;
            existingItem.SupplierId = updatedItem.SupplierId;
            existingItem.SupplierCode = updatedItem.SupplierCode;
            existingItem.SupplierPartNumber = updatedItem.SupplierPartNumber;

            existingItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return false;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
