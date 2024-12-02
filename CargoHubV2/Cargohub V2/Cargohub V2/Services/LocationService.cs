using Cargohub_V2.Contexts;

using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{ 
    public class LocationService
    {
        private CargoHubDbContext _context;

        public LocationService(CargoHubDbContext context)
        {
            _context = context;
        }

        public List<Location> GetLocations()
        {
            return _context.Locations.Take(100).ToList();
        }

        public Location? GetLocation(int locationId)
        {
            return _context.Locations.FirstOrDefault(location => location.Id == locationId);
        }

        public List<Location> GetLocationsinWarehouse(int warehouseId)
        {
            return _context.Locations.Where(location => location.WarehouseId == warehouseId).ToList();
        }

        public bool AddLocation(Location location)
        {
            if (!IsValidLocation(location))
            {
                return false;
            }

            var locationIdExists = _context.Locations.FirstOrDefault(l => l.Id == location.Id);

            if (locationIdExists != null)
            {
                return false;
            }

            location.CreatedAt = DateTime.UtcNow;
            location.UpdatedAt = DateTime.UtcNow;
            _context.Locations.Add(location);

            _context.SaveChanges();
            return true;
        }

        private bool IsValidLocation(Location location)
        {
            if (string.IsNullOrEmpty(location.Code) || string.IsNullOrEmpty(location.Name))
            {
                return false;
            }

            if (location.QuantityOnHand < 0)
            {
                return false;
            }

            var warehouseExists = _context.Warehouses.Any(w => w.Id == location.WarehouseId);
            if (!warehouseExists)
            {
                return false;
            }
            if (location.CreatedAt > DateTime.UtcNow || location.UpdatedAt > DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }


        public bool updateLocation(int locationId, Location updatedLocation)
        {
            var existingLocation = GetLocation(locationId);
            if (existingLocation != null)
            {
                if (!string.IsNullOrEmpty(updatedLocation.Code))
                    existingLocation.Code = updatedLocation.Code;

                if (!string.IsNullOrEmpty(updatedLocation.Name))
                    existingLocation.Name = updatedLocation.Name;

                if (updatedLocation.QuantityOnHand != 0)
                    existingLocation.QuantityOnHand = updatedLocation.QuantityOnHand;


                _context.Locations.Update(existingLocation);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveLocation(int locationId)
        {
            var location = GetLocation(locationId);
            if (location != null)
            {
                _context.Locations.Remove(location);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}