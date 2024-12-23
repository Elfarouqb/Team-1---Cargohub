namespace UnitTests;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Controllers;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;

[TestClass]
public class UnitTest_Shipments
{
    private CargoHubDbContext _dbContext;
    private ShipmentService _shipmentService;
    private ShipmentsController _controller;
    private IConfiguration _configuration;

    [TestInitialize]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = builder.Build();

        // Configure DbContext with the connection string
        var connectionString = _configuration.GetConnectionString("WebApiDatabase");
        var optionsBuilder = new DbContextOptionsBuilder<CargoHubDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        _dbContext = new CargoHubDbContext(optionsBuilder.Options);

        // Initialize ShipmentService and Controller with the DbContext
        _shipmentService = new ShipmentService(_dbContext);
        _controller = new ShipmentsController(_shipmentService);
    }

    private Shipment CreateDummyShipment()
    {
        // Create a new dummy shipment to use in tests with the new structure
        var dummyShipment = new Shipment
        {
            OrderId = 8,
            SourceId = 456,
            OrderDate = new DateTime(2024, 12, 13, 10, 0, 0, DateTimeKind.Utc),
            RequestDate = new DateTime(2024, 12, 14, 10, 0, 0, DateTimeKind.Utc),
            ShipmentDate = new DateTime(2024, 12, 15, 10, 0, 0, DateTimeKind.Utc),
            ShipmentType = "Standard",
            ShipmentStatus = "Pending",
            Notes = "Handle with care",
            CarrierCode = "Carrier001",
            CarrierDescription = "Carrier 1",
            ServiceCode = "Service123",
            PaymentType = "Prepaid",
            TransferMode = "Air",
            TotalPackageCount = 5,
            TotalPackageWeight = 50.5,
            Items = new List<ShipmentItem>
            {
                new ShipmentItem { ItemId = "P007435", Amount = 23 },
                new ShipmentItem { ItemId = "P009557", Amount = 1 }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Shipments.Add(dummyShipment);
        _dbContext.SaveChanges();
        return dummyShipment;
    }

    [TestMethod]
    public async Task TestGetAll()
    {
        // Act
        var result = await _controller.GetAllShipments();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var shipments = okResult.Value as List<Shipment>;
        Assert.IsNotNull(shipments);
        Assert.IsTrue(shipments.Count >= 1);
    }

    [TestMethod]
    public async Task TestGetById()
    {
        // Arrange
        var dummyShipment = CreateDummyShipment();

        // Act
        var result = await _controller.GetShipmentById(dummyShipment.Id);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var shipment = okResult.Value as Shipment;
        Assert.IsNotNull(shipment);
        Assert.AreEqual(dummyShipment.Id, shipment.Id);
    }

    [TestMethod]
    public async Task TestPost()
    {
        // Arrange
        var newShipment = new Shipment
        {
            OrderId = 8888,
            SourceId = 457,
            OrderDate = new DateTime(2024, 12, 16, 10, 0, 0, DateTimeKind.Utc),
            RequestDate = new DateTime(2024, 12, 17, 10, 0, 0, DateTimeKind.Utc),
            ShipmentDate = new DateTime(2024, 12, 18, 10, 0, 0, DateTimeKind.Utc),
            ShipmentType = "Express",
            ShipmentStatus = "New",
            Notes = "Fragile",
            CarrierCode = "Carrier002",
            CarrierDescription = "Carrier 2",
            ServiceCode = "Service456",
            PaymentType = "Collect",
            TransferMode = "Sea",
            TotalPackageCount = 3,
            TotalPackageWeight = 30.0,
            Items = new List<ShipmentItem>
            {
                new ShipmentItem { ItemId = "P007999", Amount = 5 }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _controller.CreateShipment(newShipment);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);

        var shipment = createdResult.Value as Shipment;
        Assert.IsNotNull(shipment);
        Assert.AreEqual(8888, shipment.OrderId);
    }

    [TestMethod]
    public async Task TestPut()
    {
        // Arrange
        var dummyShipment = CreateDummyShipment();
        var updatedShipment = new Shipment
        {
            Id = dummyShipment.Id,
            OrderId = dummyShipment.OrderId,
            ShipmentStatus = "Updated",
            Items = dummyShipment.Items,
            ShipmentType = dummyShipment.ShipmentType,
            ShipmentDate = dummyShipment.ShipmentDate,
            SourceId = dummyShipment.SourceId,
            Notes = dummyShipment.Notes,
            CarrierCode = dummyShipment.CarrierCode,
            CarrierDescription = dummyShipment.CarrierDescription,
            ServiceCode = dummyShipment.ServiceCode,
            PaymentType = dummyShipment.PaymentType,
            TransferMode = dummyShipment.TransferMode,
            TotalPackageCount = dummyShipment.TotalPackageCount,
            TotalPackageWeight = dummyShipment.TotalPackageWeight,
            CreatedAt = dummyShipment.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _controller.UpdateShipment(dummyShipment.Id, updatedShipment);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var shipment = okResult.Value as Shipment;
        Assert.IsNotNull(shipment);
        Assert.AreEqual("Updated", shipment.ShipmentStatus);
    }

    [TestMethod]
    public async Task TestDelete()
    {
        // Arrange
        var dummyShipment = CreateDummyShipment();

        // Act
        var result = await _controller.RemoveShipment(dummyShipment.Id);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}

