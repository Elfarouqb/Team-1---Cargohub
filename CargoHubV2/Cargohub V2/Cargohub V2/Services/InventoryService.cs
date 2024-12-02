using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class InventoryService 
    {
        private CargoHubDbContext _context;
    
        public InventoryService(CargoHubDbContext context)
        {
            _context = context;
        }

        public List<Inventory> GetAllInventories()
        {
            return _context.Inventories.Take(100).ToList();
            
        }
        
        public Inventory? GetInventoryByID(int id)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == id);
        }

        public List<Inventory> GetInventoriesForItem(string itemId)
        {
            return _context.Inventories.Where(i => i.ItemId == itemId).ToList();
        }
        public Inventory? AddInventory(Inventory inventory)
        {
            if (!InventoryFormatvalid(inventory))
            {
                return null;
            }

            var inventoryData = _context.Inventories.FirstOrDefault(i => i.Id == inventory.Id);
            if (inventoryData != null)
            {
                return null;
            }
         

            _context.Inventories.Add(inventory);
            _context.SaveChanges();
            return inventory;
        }

        private bool InventoryFormatvalid(Inventory inventory)
        {
            if (string.IsNullOrEmpty(inventory.ItemId))
            {
                return false;
            }

            if (inventory.TotalOnHand < 0 || inventory.TotalOrdered < 0 || inventory.TotalAllocated < 0 || inventory.TotalAvailable < 0)
            {

                return false;
            }
            if (inventory.Locations != null && inventory.Locations.Any(id => id <= 0))
            {
                return false;
            }

            if (inventory.CreatedAt > DateTime.UtcNow || inventory.UpdatedAt > DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public Inventory? UpdateInventory(int id, Inventory inventory)
        {
            var inventoryData = _context.Inventories.FirstOrDefault(i => i.Id == id);
            if (inventoryData == null)
            {
                return null;
            }
            
            if (!string.IsNullOrEmpty(inventory.ItemReference)) 
                inventoryData.ItemReference = inventory.ItemReference;
            if (!string.IsNullOrEmpty(inventory.Description)) 
                inventoryData.Description = inventory.Description;
            if (inventory.TotalOrdered != 0) 
                inventoryData.TotalOrdered = inventory.TotalOrdered;
            if (inventory.TotalAllocated != 0) 
                inventoryData.TotalAllocated = inventory.TotalAllocated;
            if (inventory.TotalAvailable != 0) 
                inventoryData.TotalAvailable = inventory.TotalAvailable;
            if (inventory.Locations != null) 
                inventoryData.Locations = inventory.Locations;

            _context.Inventories.Update(inventoryData);
            _context.SaveChanges();
            return inventoryData;
        }

        public void UpdateLocationStock(int inventoryId)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);
            if (inventory == null)
            {
                throw new Exception("Inventory not found.");
            }

            foreach (var locationId in inventory.Locations)
            {
                var location = _context.Locations.FirstOrDefault(l => l.Id == locationId);
                if (location != null)
                {
                    location.QuantityOnHand = inventory.TotalOnHand;
                    _context.Locations.Update(location);
                }
            }
            _context.SaveChanges();
        }

        public bool DeleteInventory(int id)
        {
            var inventoryToBeDeleted = GetInventoryByID(id);

            if (inventoryToBeDeleted != null)
            {
                _context.Inventories.Remove(inventoryToBeDeleted);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


    }
}
