using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cargohub_V2.Services
{
    public class ShipmentService
    {
        private readonly CargoHubDbContext _context;

        public ShipmentService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> GetAllShipmentsAsync()
        {
            return await _context.Shipments
                .Include(s => s.Items) // Load the related ShipmentItems
                .ToListAsync();
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int shipmentId)
        {
            return await _context.Shipments
                .Include(s => s.Items) // Load the related ShipmentItems
                .FirstOrDefaultAsync(s => s.Id == shipmentId);
        }

        public async Task<List<ShipmentItem>> GetItemsInShipmentAsync(int shipmentId)
        {
            var shipment = await GetShipmentByIdAsync(shipmentId);
            return shipment?.Items.ToList() ?? new List<ShipmentItem>(); // Convert ICollection to List explicitly
        }


        public async Task<Shipment> AddShipmentAsync(Shipment newShipment)
        {
            // Add timestamps
            newShipment.CreatedAt = DateTime.UtcNow;
            newShipment.UpdatedAt = DateTime.UtcNow;

            // Ensure that the OrderId is unique
            var existingShipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.OrderId == newShipment.OrderId); // Check for existing shipment with the same OrderId

            if (existingShipment != null)
            {
                // If a shipment with the same OrderId already exists, return or throw an error
                throw new InvalidOperationException("A shipment with the same OrderId already exists.");
            }

            // Add the shipment to the database
            _context.Shipments.Add(newShipment);
            await _context.SaveChangesAsync(); // Ensure `Id` is generated here

            // Split the OrderId string into individual order IDs
            var orderIds = newShipment.OrderId.Split(',').Select(id => id.Trim()).ToList();

            // Update the ShipmentId in the Orders table for each OrderId
            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id.ToString() == orderId);  // Ensure OrderId is string-based

                if (order != null)
                {
                    order.ShipmentId = newShipment.Id;  // Update ShipmentId in the Orders table
                }
            }

            // Save changes to the Orders table
            await _context.SaveChangesAsync();

            // Assign the shipment's auto-generated Id to its items
            if (newShipment.Items != null)
            {
                foreach (var item in newShipment.Items)
                {
                    item.ShipmentId = newShipment.Id;
                }

                // Save again if items were updated
                await _context.SaveChangesAsync();
            }

            return newShipment;
        }




        public async Task<bool> UpdateShipmentAsync(int shipmentId, Shipment updatedShipment)
        {
            var existingShipment = await _context.Shipments
                .Include(s => s.Items)  // Make sure to include the Items collection
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (existingShipment == null)
            {
                return false;
            }

            // Update the existing shipment properties
            existingShipment.OrderId = updatedShipment.OrderId;
            existingShipment.SourceId = updatedShipment.SourceId;
            existingShipment.OrderDate = updatedShipment.OrderDate;
            existingShipment.RequestDate = updatedShipment.RequestDate;
            existingShipment.ShipmentDate = updatedShipment.ShipmentDate;
            existingShipment.ShipmentType = updatedShipment.ShipmentType;
            existingShipment.ShipmentStatus = updatedShipment.ShipmentStatus;
            existingShipment.Notes = updatedShipment.Notes;
            existingShipment.CarrierCode = updatedShipment.CarrierCode;
            existingShipment.CarrierDescription = updatedShipment.CarrierDescription;
            existingShipment.ServiceCode = updatedShipment.ServiceCode;
            existingShipment.PaymentType = updatedShipment.PaymentType;
            existingShipment.TransferMode = updatedShipment.TransferMode;
            existingShipment.TotalPackageCount = updatedShipment.TotalPackageCount;
            existingShipment.TotalPackageWeight = updatedShipment.TotalPackageWeight;

            // Convert ICollection<ShipmentItem> to List<ShipmentItem> and assign
            existingShipment.Items = updatedShipment.Items.ToList(); // This fixes the error

            existingShipment.UpdatedAt = DateTime.UtcNow;

            // Update the corresponding Order in the Orders table
            var orderIds = updatedShipment.OrderId.Split(',').Select(id => id.Trim()).ToList();

            // Update the ShipmentId in the Orders table for each OrderId
            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id.ToString() == orderId);  // Ensure OrderId is string-based

                if (order != null)
                {
                    order.ShipmentId = updatedShipment.Id;  // Update ShipmentId in the Orders table
                }
            }

            // Save changes to the Orders table

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> UpdateItemsInShipmentAsync(int shipmentId, List<ShipmentItem> updatedItems)
        {
            var shipment = await GetShipmentByIdAsync(shipmentId);

            if (shipment == null)
            {
                return false;
            }

            // Clear existing items and add the new ones
            shipment.Items.Clear();

            foreach (var item in updatedItems)
            {
                shipment.Items.Add(item);
            }

            shipment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RemoveShipmentAsync(int shipmentId)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null)
            {
                return false;
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}