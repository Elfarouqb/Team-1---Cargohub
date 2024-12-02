using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Services
{
    public class SuppliersService
    {
        private readonly CargoHubDbContext _context;

        public SuppliersService(CargoHubDbContext context)
        {
            _context = context;
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.Take(100).ToList();
        }

        public Supplier? GetSupplierById(int supplierId)
        {
            return _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
        }


        public Supplier? AddSupplier(Supplier supplier)
        {
            if (!IsSupplierValid(supplier))
            {
                return null; 
            }

            var existingSupplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplier.Id);


            if(existingSupplier != null)
            {
                return null;
                
            }

            supplier.CreatedAt = DateTime.UtcNow;
            supplier.UpdatedAt = DateTime.UtcNow;

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }
        public Supplier? UpdateSupplier(int supplierId, Supplier updatedSupplier)
        {
            var existingSupplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (existingSupplier == null)
            {
                return null; 
            }

            if (!string.IsNullOrEmpty(updatedSupplier.Code))
                existingSupplier.Code = updatedSupplier.Code;

            if (!string.IsNullOrEmpty(updatedSupplier.Name))
                existingSupplier.Name = updatedSupplier.Name;

            if (!string.IsNullOrEmpty(updatedSupplier.Address))
                existingSupplier.Address = updatedSupplier.Address;

            if (!string.IsNullOrEmpty(updatedSupplier.AddressExtra))
                existingSupplier.AddressExtra = updatedSupplier.AddressExtra;

            if (!string.IsNullOrEmpty(updatedSupplier.City))
                existingSupplier.City = updatedSupplier.City;

            if (!string.IsNullOrEmpty(updatedSupplier.ZipCode))
                existingSupplier.ZipCode = updatedSupplier.ZipCode;

            if (!string.IsNullOrEmpty(updatedSupplier.Province))
                existingSupplier.Province = updatedSupplier.Province;

            if (!string.IsNullOrEmpty(updatedSupplier.Country))
                existingSupplier.Country = updatedSupplier.Country;

            if (!string.IsNullOrEmpty(updatedSupplier.ContactName))
                existingSupplier.ContactName = updatedSupplier.ContactName;

            if (!string.IsNullOrEmpty(updatedSupplier.PhoneNumber))
                existingSupplier.PhoneNumber = updatedSupplier.PhoneNumber;

            if (!string.IsNullOrEmpty(updatedSupplier.Reference))
                existingSupplier.Reference = updatedSupplier.Reference;


            existingSupplier.UpdatedAt = DateTime.UtcNow;

            _context.Suppliers.Update(existingSupplier);
            _context.SaveChanges();
            return existingSupplier;
        }

        public bool DeleteSupplier(int supplierId)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsSupplierValid(Supplier supplier)
        {
            if (supplier.Id <= 0 || string.IsNullOrEmpty(supplier.Code) || string.IsNullOrEmpty(supplier.Name) || string.IsNullOrEmpty(supplier.Address) ||string.IsNullOrEmpty(supplier.City))
            {
                
                return false;
            }

            if (string.IsNullOrEmpty(supplier.ZipCode) || string.IsNullOrEmpty(supplier.Country) || string.IsNullOrEmpty(supplier.ContactName) ||string.IsNullOrEmpty(supplier.PhoneNumber)){
                return false;
            }


            if (supplier.CreatedAt > DateTime.UtcNow || supplier.UpdatedAt > DateTime.UtcNow)
            {
                return false; 
            }

            return true; 
        }

        public bool AddProductPrice(int supplierId, int price)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            if(supplier == null)
            {
                return false; 
            }

            if (price <= 0)
            {
                return false; 
            }

            supplier.Price = price;

            _context.Suppliers.Update(supplier);
            _context.SaveChanges();

            return true; 
        }


    }
}