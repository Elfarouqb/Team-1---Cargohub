using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;

namespace Cargohub_V2.Services
{
    public class SuppliersService
    {
        private readonly CargoHubDbContext _context;

        // Constructor to inject CargoHubDbContext
        public SuppliersService(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByCodeAsync(string code)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            DateTime createdAt = DateTime.UtcNow;
            DateTime updatedAt = DateTime.UtcNow;

            supplier.CreatedAt = new DateTime(createdAt.Year, createdAt.Month, createdAt.Day, createdAt.Hour, createdAt.Minute, createdAt.Second, DateTimeKind.Utc);
            supplier.UpdatedAt = new DateTime(updatedAt.Year, updatedAt.Month, updatedAt.Day, updatedAt.Hour, updatedAt.Minute, updatedAt.Second, DateTimeKind.Utc);

            // Add the new supplier
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier supplier, string code)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Code == code);

            if (existingSupplier == null)
            {
                return null;
            }

            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;
            existingSupplier.AddressExtra = supplier.AddressExtra;
            existingSupplier.City = supplier.City;
            existingSupplier.ZipCode = supplier.ZipCode;
            existingSupplier.Province = supplier.Province;
            existingSupplier.Country = supplier.Country;
            existingSupplier.ContactName = supplier.ContactName;
            existingSupplier.PhoneNumber = supplier.PhoneNumber;
            existingSupplier.Reference = supplier.Reference;

            DateTime updatedAt = DateTime.UtcNow;
            existingSupplier.UpdatedAt = new DateTime(updatedAt.Year, updatedAt.Month, updatedAt.Day, updatedAt.Hour, updatedAt.Minute, updatedAt.Second, DateTimeKind.Utc);

            _context.Suppliers.Update(existingSupplier);
            await _context.SaveChangesAsync();

            return existingSupplier;
        }

        public async Task<Supplier> RemoveSupplierByCodeAsync(string code)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Code == code);

            if (supplier == null)
            {
                return null;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }
    }
}
