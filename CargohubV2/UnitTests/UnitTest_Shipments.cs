using Cargohub_V2.Controllers;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cargohub_V2.Tests
{
    public class ShipmentsControllerTests
    {
        private readonly Mock<ShipmentService> _mockService;
        private readonly ShipmentsController _controller;

        public ShipmentsControllerTests()
        {
            _mockService = new Mock<ShipmentService>();
            _controller = new ShipmentsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllShipments_ReturnsOkResult_WithListOfShipments()
        {
            // Arrange
            var mockShipments = new List<Shipment>
            {
                new Shipment { Id = 1, OrderId = 101, ShipmentStatus = "Pending" },
                new Shipment { Id = 2, OrderId = 102, ShipmentStatus = "Shipped" }
            };
            _mockService.Setup(s => s.GetAllShipmentsAsync()).ReturnsAsync(mockShipments);

            // Act
            var result = await _controller.GetAllShipments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var shipments = Assert.IsType<List<Shipment>>(okResult.Value);
            Assert.Equal(2, shipments.Count);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsOkResult_WhenShipmentExists()
        {
            // Arrange
            var mockShipment = new Shipment { Id = 1, OrderId = 101, ShipmentStatus = "Pending" };
            _mockService.Setup(s => s.GetShipmentByIdAsync(1)).ReturnsAsync(mockShipment);

            // Act
            var result = await _controller.GetShipmentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var shipment = Assert.IsType<Shipment>(okResult.Value);
            Assert.Equal(1, shipment.Id);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetShipmentByIdAsync(1)).ReturnsAsync((Shipment)null);

            // Act
            var result = await _controller.GetShipmentById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetItemsInShipment_ReturnsOkResult_WithListOfItems()
        {
            // Arrange
            var mockItems = new List<ShipmentStock>
            {
                new ShipmentStock { ItemId = "P001", Amount = 2 },
                new ShipmentStock { ItemId = "P002", Amount = 1 }
            };
            _mockService.Setup(s => s.GetItemsInShipmentAsync(1)).ReturnsAsync(mockItems);

            // Act
            var result = await _controller.GetItemsInShipment(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<ShipmentStock>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task AddShipment_ReturnsCreatedAtActionResult_WithCreatedShipment()
        {
            // Arrange
            var newShipment = new Shipment { Id = 3, OrderId = 103, ShipmentStatus = "Scheduled" };
            _mockService.Setup(s => s.AddShipmentAsync(newShipment)).ReturnsAsync(newShipment);

            // Act
            var result = await _controller.AddShipment(newShipment);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdShipment = Assert.IsType<Shipment>(createdAtActionResult.Value);
            Assert.Equal(3, createdShipment.Id);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updatedShipment = new Shipment { Id = 1, OrderId = 101, ShipmentStatus = "Delivered" };
            _mockService.Setup(s => s.UpdateShipmentAsync(1, updatedShipment)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateShipment(1, updatedShipment);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            var updatedShipment = new Shipment { Id = 1, OrderId = 101, ShipmentStatus = "Delivered" };
            _mockService.Setup(s => s.UpdateShipmentAsync(1, updatedShipment)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateShipment(1, updatedShipment);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            _mockService.Setup(s => s.RemoveShipmentAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveShipment(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.RemoveShipmentAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveShipment(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
