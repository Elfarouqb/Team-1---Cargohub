using Cargohub_V2.Controllers;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Shipment_Test
    {
        private readonly Mock<ShipmentService> _mockShipmentService;
        private readonly ShipmentsController _controller;

        public Shipment_Test()
        {
            _mockShipmentService = new Mock<ShipmentService>(null);
            _controller = new ShipmentsController(_mockShipmentService.Object);
        }

        [Fact]
        public async Task GetAllShipments_ReturnsOkResult_WithListOfShipments()
        {
            // Arrange
            var shipments = new List<Shipment> { new Shipment { Id = 1 }, new Shipment { Id = 2 } };
            _mockShipmentService.Setup(service => service.GetAllShipmentsAsync()).ReturnsAsync(shipments);

            // Act
            var result = await _controller.GetAllShipments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnShipments = Assert.IsType<List<Shipment>>(okResult.Value);
            Assert.Equal(2, returnShipments.Count);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            _mockShipmentService.Setup(service => service.GetShipmentByIdAsync(It.IsAny<int>())).ReturnsAsync((Shipment)null);

            // Act
            var result = await _controller.GetShipmentById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetShipmentById_ReturnsOkResult_WithShipment()
        {
            // Arrange
            var shipment = new Shipment { Id = 1 };
            _mockShipmentService.Setup(service => service.GetShipmentByIdAsync(It.IsAny<int>())).ReturnsAsync(shipment);

            // Act
            var result = await _controller.GetShipmentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnShipment = Assert.IsType<Shipment>(okResult.Value);
            Assert.Equal(1, returnShipment.Id);
        }

        [Fact]
        public async Task CreateShipment_ReturnsBadRequest_WhenShipmentIsNull()
        {
            // Act
            var result = await _controller.CreateShipment(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Shipment cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateShipment_ReturnsCreatedAtAction_WithCreatedShipment()
        {
            // Arrange
            var shipment = new Shipment { Id = 1 };
            _mockShipmentService.Setup(service => service.AddShipmentAsync(It.IsAny<Shipment>())).ReturnsAsync(shipment);

            // Act
            var result = await _controller.CreateShipment(shipment);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnShipment = Assert.IsType<Shipment>(createdAtActionResult.Value);
            Assert.Equal(1, returnShipment.Id);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsBadRequest_WhenShipmentIsNull()
        {
            // Act
            var result = await _controller.UpdateShipment(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid shipment data.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            _mockShipmentService.Setup(service => service.UpdateShipmentAsync(It.IsAny<int>(), It.IsAny<Shipment>())).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateShipment(1, new Shipment());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Shipment with ID 1 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNotFound_WhenShipmentDoesNotExist()
        {
            // Arrange
            _mockShipmentService.Setup(service => service.RemoveShipmentAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveShipment(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveShipment_ReturnsNoContent_WhenShipmentIsRemoved()
        {
            // Arrange
            _mockShipmentService.Setup(service => service.RemoveShipmentAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveShipment(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}