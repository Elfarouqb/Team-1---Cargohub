using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Shipment_Test
    {
        private CargoHubDbContext _dbContext;
        private ShipmentService _shipmentService;

        public Shipment_Test()
        {
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCargoHubDatabase")
                .Options;

            _dbContext = new CargoHubDbContext(options);
            SeedDatabase(_dbContext);
            _shipmentService = new ShipmentService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed Shipments
            context.Shipments.Add(new Shipment
            {
                Id = 1,
                SourceId = 1,
                OrderId = "4",
                OrderDate = DateTime.UtcNow.AddDays(-5).ToString("o"),
                RequestDate = DateTime.UtcNow.AddDays(-3).ToString("o"),
                ShipmentDate = DateTime.UtcNow.AddDays(-1).ToString("o"),
                ShipmentType = "Express",
                ShipmentStatus = "Shipped",
                Notes = "Test shipment",
                CarrierCode = "UPS",
                CarrierDescription = "UPS Express",
                ServiceCode = "EXP",
                PaymentType = "Prepaid",
                TransferMode = "Air",
                TotalPackageCount = 2,
                TotalPackageWeight = 10.5,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            });

            context.Shipments.Add(new Shipment
            {
                Id = 2,
                SourceId = 2,
                OrderId = "1,2,3",
                OrderDate = DateTime.UtcNow.AddDays(-7).ToString("o"),
                RequestDate = DateTime.UtcNow.AddDays(-4).ToString("o"),
                ShipmentDate = DateTime.UtcNow.AddDays(-2).ToString("o"),
                ShipmentType = "Standard",
                ShipmentStatus = "Pending",
                Notes = "Another test shipment",
                CarrierCode = "FedEx",
                CarrierDescription = "FedEx Standard",
                ServiceCode = "STP",
                PaymentType = "Collect",
                TransferMode = "Truck",
                TotalPackageCount = 5,
                TotalPackageWeight = 25.0,
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                UpdatedAt = DateTime.UtcNow.AddDays(-7)
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllShipments_ReturnsOkResult_WithListOfShipments()
        {
            // Act
            var result = await _shipmentService.GetAllShipmentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Act
            var result = await _shipmentService.GetShipmentByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsOkResult_WithShipment()
        {
            // Act
            var result = await _shipmentService.GetShipmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateShipment_ReturnsCreatedAtAction_WithCreatedShipment()
        {
            // Arrange
            var newShipment = new Shipment
            {
                Id = 3,
                SourceId = 3,
                OrderId = "15,27",
                OrderDate = DateTime.UtcNow.ToString("o"),
                RequestDate = DateTime.UtcNow.ToString("o"),
                ShipmentDate = DateTime.UtcNow.ToString("o"),
                ShipmentType = "Standard",
                ShipmentStatus = "Pending",
                Notes = "New shipment",
                CarrierCode = "DHL",
                CarrierDescription = "DHL Standard",
                ServiceCode = "STF",
                PaymentType = "Prepaid",
                TransferMode = "Truck",
                TotalPackageCount = 3,
                TotalPackageWeight = 15.0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _shipmentService.AddShipmentAsync(newShipment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            var updatedShipment = new Shipment
            {
                Id = 999,
                SourceId = 1,
                OrderId = "1,2,3,4",
                OrderDate = DateTime.UtcNow.ToString("o"),
                RequestDate = DateTime.UtcNow.ToString("o"),
                ShipmentDate = DateTime.UtcNow.ToString("o"),
                ShipmentType = "Express",
                ShipmentStatus = "Delivered",
                Notes = "Updated shipment",
                CarrierCode = "UPS",
                CarrierDescription = "UPS Updated",
                ServiceCode = "EXP",
                PaymentType = "Prepaid",
                TransferMode = "Air",
                TotalPackageCount = 3,
                TotalPackageWeight = 12.5,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _shipmentService.UpdateShipmentAsync(999, updatedShipment);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsOkResult_WhenShipmentIsUpdated()
        {
            // Arrange
            var updatedShipment = new Shipment
            {
                Id = 1,
                SourceId = 1,
                OrderId = "1,2,3,4",
                OrderDate = DateTime.UtcNow.ToString("o"),
                RequestDate = DateTime.UtcNow.ToString("o"),
                ShipmentDate = DateTime.UtcNow.ToString("o"),
                ShipmentType = "Express",
                ShipmentStatus = "Delivered",
                Notes = "Updated shipment",
                CarrierCode = "UPS",
                CarrierDescription = "UPS Updated",
                ServiceCode = "EXP",
                PaymentType = "Prepaid",
                TransferMode = "Air",
                TotalPackageCount = 3,
                TotalPackageWeight = 12.5,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _shipmentService.UpdateShipmentAsync(1, updatedShipment);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Act
            var result = await _shipmentService.RemoveShipmentAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNoContent_WhenShipmentIsRemoved()
        {
            // Act
            var result = await _shipmentService.RemoveShipmentAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}