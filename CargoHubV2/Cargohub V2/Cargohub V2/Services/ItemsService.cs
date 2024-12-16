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
                .OrderBy(i => i.Id) // Order by Id in ascending order
                .Take(10)
                .ToListAsync();
        }


        public async Task<Item> GetItemByUidAsync(string uid)
        {
            return await _context.Items
                .Include(i => i.ItemLine)
                .Include(i => i.ItemGroup)
                .Include(i => i.ItemType)
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.UId == uid);
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


        public async Task<Item?> GetItemsBySupplierAsync(int supplierId)
        {
            return await _context.Items
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Supplier.Id == supplierId);
        }
        public async Task<Item?> GetItemByCodeAsync(string code)
        {
            return await _context.Items
                .Include(i => i.Code)
                .FirstOrDefaultAsync(i => i.Code == code);
        }

        public async Task<Item> AddItemAsync(Item newItem)
        {
            System.Console.WriteLine(newItem.Id);
            // Validate SupplierId
            var supplierExists = await _context.Suppliers.FindAsync(newItem.SupplierId); //(s => s.Id == newItem.SupplierId);
            if (supplierExists == null)

            {
                throw new Exception($"Supplier with ID {newItem.SupplierId} does not exist.");
            }

            // Get the latest UID
            var lastItem = await _context.Items
                .OrderByDescending(i => i.UId)
                .FirstOrDefaultAsync();

            // Generate UID (increment from last UID)
            if (lastItem != null)
            {
                var lastUidNumericPart = int.Parse(lastItem.UId.Substring(1)); // Remove 'P' and parse number
                newItem.UId = $"P{lastUidNumericPart + 1:D6}"; // Increment and format as P###### (e.g., P000002)
            }
            else
            {
                newItem.UId = "P000001"; // First UID
            }

            // Generate Code (random alphanumeric string)
            newItem.Code = GenerateUniqueCode();

            DateTime CreatedAt = DateTime.UtcNow;
            DateTime UpdatedAt = DateTime.UtcNow;

            newItem.CreatedAt = new DateTime(CreatedAt.Year, CreatedAt.Month, CreatedAt.Day, CreatedAt.Hour, CreatedAt.Minute, CreatedAt.Second, DateTimeKind.Utc);
            newItem.UpdatedAt = new DateTime(UpdatedAt.Year, UpdatedAt.Month, UpdatedAt.Day, UpdatedAt.Hour, UpdatedAt.Minute, UpdatedAt.Second, DateTimeKind.Utc);

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }

        public async Task<Item> UpdateItemAsync(int id, Item updatedItem)
        {
            var existingItem = await _context.Items.FindAsync(id);

            if (existingItem == null)
            {
                return null;
            }


            var supplierExists = await _context.Suppliers.AnyAsync(s => s.Id == updatedItem.SupplierId);
            if (!supplierExists)
            {
                throw new Exception($"Supplier with ID {updatedItem.SupplierId} does not exist.");
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

            DateTime UpdatedAt = DateTime.UtcNow;
            existingItem.UpdatedAt = new DateTime(UpdatedAt.Year, UpdatedAt.Month, UpdatedAt.Day, UpdatedAt.Hour, UpdatedAt.Minute, UpdatedAt.Second, DateTimeKind.Utc);

            await _context.SaveChangesAsync();
            return updatedItem;
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


        private string GenerateUniqueCode()
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(0, 9).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }
}
