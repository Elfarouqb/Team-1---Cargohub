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
            newShipment.CreatedAt = DateTime.UtcNow;
            newShipment.UpdatedAt = DateTime.UtcNow;

            // Ensure shipmentId is set for each item
            if (newShipment.Items != null)
            {
                foreach (var item in newShipment.Items)
                {
                    item.ShipmentId = newShipment.Id;  // Set the ShipmentId explicitly
                }
            }

            _context.Shipments.Add(newShipment);
            await _context.SaveChangesAsync();
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