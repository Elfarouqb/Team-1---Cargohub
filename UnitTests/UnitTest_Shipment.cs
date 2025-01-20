using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;  // Use xUnit namespace

namespace UnitTests
{
    public class UnitTest_Shipment  // Remove [TestClass] since xUnit doesn't need this
    {
        private CargoHubDbContext _dbContext;
        private ShipmentService _shipmentService;

        public UnitTest_Shipment()  // Constructor used for setup in xUnit
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

        [Fact]  // Replace [TestMethod] with xUnit's [Fact]
        public async Task TestAddShipment()
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
            Assert.NotNull(result);  // Use xUnit's Assert
            Assert.Equal(newShipment.OrderId, result.OrderId);  // Use xUnit's Assert
        }

        [Fact]  // Replace [TestMethod] with xUnit's [Fact]
        public async Task TestUpdateShipment()
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
            Assert.True(result);  // Use xUnit's Assert
        }

        [Theory]  // Use xUnit's [Theory] for parameterized tests
        [InlineData(1, true)]  // Test with an existing shipment ID
        [InlineData(999, false)]  // Test with a non-existent shipment ID
        public async Task TestDeleteShipment(int shipmentId, bool shouldDelete)
        {
            // Act
            var result = await _shipmentService.RemoveShipmentAsync(shipmentId);

            // Assert
            Assert.Equal(result, shouldDelete);  // Use xUnit's Assert
        }
    }
}
