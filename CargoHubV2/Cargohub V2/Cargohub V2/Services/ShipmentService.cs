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
                .Include(s => s.Items) //Load ShipmentItems
                .ToListAsync();
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int shipmentId)
        {
            return await _context.Shipments
                .Include(s => s.Items) //Load ShipmentItems
                .FirstOrDefaultAsync(s => s.Id == shipmentId);
        }

        public async Task<List<ShipmentItem>> GetItemsInShipmentAsync(int shipmentId)
        {
            var shipment = await GetShipmentByIdAsync(shipmentId);
            return shipment?.Items.ToList() ?? new List<ShipmentItem>(); //convert to list
        }


        public async Task<Shipment> AddShipmentAsync(Shipment newShipment)
        {
            newShipment.CreatedAt = DateTime.UtcNow;
            newShipment.UpdatedAt = DateTime.UtcNow;

            //Add the shipment to the database
            _context.Shipments.Add(newShipment);
            await _context.SaveChangesAsync();

            //Split OrderId string
            var orderIds = newShipment.OrderId.Split(',').Select(id => id.Trim()).ToList();

            //Update the ShipmentId in the Orders and create ShipmentItems van de Orders
            var shipmentItems = new List<ShipmentItem>();
            foreach (var orderId in orderIds)
            {
                //Orders matching the OrderId
                var orderItems = await _context.OrderItems.Where(oi => oi.OrderId.ToString() == orderId).ToListAsync();

                foreach (var orderItem in orderItems)
                {
                    //Create ShipmentItem for each OrderItem
                    shipmentItems.Add(new ShipmentItem
                    {
                        ShipmentId = newShipment.Id,
                        ItemId = orderItem.ItemId,
                        Amount = orderItem.Amount
                    });
                }
            }

            //Add ShipmentItems to database
            _context.ShipmentItems.AddRange(shipmentItems);
            await _context.SaveChangesAsync();

            return newShipment;
        }



        public async Task<bool> UpdateShipmentAsync(int shipmentId, Shipment updatedShipment)
        {
            //Get shipment
            var existingShipment = await _context.Shipments.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (existingShipment == null)
            {
                throw new Exception($"Shipment with ID {shipmentId} does not exist.");
            }

            //Update shipment properties
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
            existingShipment.UpdatedAt = DateTime.UtcNow;

            //Update ShipmentItems for new OrderId
            var orderIds = updatedShipment.OrderId.Split(',').Select(id => id.Trim()).ToList();

            //Remove old ShipmentItems
            var existingItems = _context.ShipmentItems.Where(si => si.ShipmentId == shipmentId);
            _context.ShipmentItems.RemoveRange(existingItems);

            //Create new ShipmentItems
            var shipmentItems = new List<ShipmentItem>();
            foreach (var orderId in orderIds)
            {
                //Get orders matching OrderId
                var orderItems = await _context.OrderItems.Where(oi => oi.OrderId.ToString() == orderId).ToListAsync();

                foreach (var orderItem in orderItems)
                {
                    //Create ShipmentItem for each OrderItem
                    shipmentItems.Add(new ShipmentItem
                    {
                        ShipmentId = shipmentId,
                        ItemId = orderItem.ItemId,
                        Amount = orderItem.Amount
                    });
                }
            }

            //Add new ShipmentItems
            _context.ShipmentItems.AddRange(shipmentItems);

            //Save changes
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error saving shipment changes: {ex.InnerException?.Message ?? ex.Message}");
            }
        }



        public async Task<bool> UpdateItemsInShipmentAsync(int shipmentId, List<ShipmentItem> updatedItems)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Items) // Include existing items for modification
                .FirstOrDefaultAsync(s => s.Id == shipmentId);

            if (shipment == null)
            {
                return false;
            }

            // Remove existing items explicitly
            _context.ShipmentItems.RemoveRange(shipment.Items);

            // Add new items
            foreach (var item in updatedItems)
            {
                item.ShipmentId = shipmentId; // Ensure proper association
                _context.ShipmentItems.Add(item);
            }

            // Update the shipment's timestamp
            shipment.UpdatedAt = DateTime.UtcNow;

            // Save changes
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