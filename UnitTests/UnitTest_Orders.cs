using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UnitTest_Order
    {
        private CargoHubDbContext _dbContext;
        private OrderService _orderService;

        public UnitTest_Order()
        {
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestOrdersbDatabase")
                .Options;

            _dbContext = new CargoHubDbContext(options);
            SeedDatabase(_dbContext);
            _orderService = new OrderService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            context.Orders.AddRange(
                new Order { Id = 1, ShipTo = "ClientA", BillTo = "ClientA", ShipmentId = 1, TotalAmount = 100.0, TotalTax = 10.0 },
                new Order { Id = 2, ShipTo = "ClientB", BillTo = "ClientB", ShipmentId = 1, TotalAmount = 200.0, TotalTax = 20.0 },
                new Order { Id = 3, ShipTo = "ClientC", BillTo = "ClientC", ShipmentId = 2, TotalAmount = 150.0, TotalTax = 15.0 },
                new Order { Id = 4, ShipTo = "ClientD", BillTo = "ClientD", ShipmentId = 2, TotalAmount = 180.0, TotalTax = 18.0 }
            );

            context.OrderItems.AddRange(
                new OrderItem { OrderId = 1, ItemId = "Item001", Amount = 5 },
                new OrderItem { OrderId = 1, ItemId = "Item002", Amount = 3 },
                new OrderItem { OrderId = 2, ItemId = "Item003", Amount = 2 },
                new OrderItem { OrderId = 3, ItemId = "Item004", Amount = 7 },
                new OrderItem { OrderId = 4, ItemId = "Item005", Amount = 6 }
            );

            context.SaveChanges();
        }

        [Fact]
        public async Task TestGetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            Assert.NotNull(orders);
            Assert.Equal(4, orders.Count);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        public async Task TestGetOrderByIdAsync(int orderId, bool exists)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (exists)
            {
                Assert.NotNull(order);
                Assert.Equal(orderId, order.Id);
            }
            else
            {
                Assert.Null(order);
            }
        }

        [Fact]
        public async Task TestGetItemsInOrderAsync()
        {
            var items = await _orderService.GetItemsInOrderAsync(1);

            Assert.NotNull(items);
            Assert.Equal(2, items.Count); //Orderid 1 has 2 items

            Assert.Contains(items, item => item.ItemId == "Item001" && item.Amount == 5);
            Assert.Contains(items, item => item.ItemId == "Item002" && item.Amount == 3);
        }

        [Fact]
        public async Task TestAddOrderAsync()
        {
            var newOrder = new Order
            {
                Id = 5,
                ShipTo = "ClientE",
                BillTo = "ClientE",
                ShipmentId = 3,
                OrderDate = DateTime.UtcNow,
                RequestDate = DateTime.UtcNow,
                TotalAmount = 100.0,
                TotalTax = 10.0,
                TotalDiscount = 5.0,
                TotalSurcharge = 2.5,
            };

            var result = await _orderService.AddOrderAsync(newOrder);

            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("ClientE", result.ShipTo);
            Assert.Equal(100.0, result.TotalAmount);
        }

        [Fact]
        public async Task TestUpdateOrderAsync()
        {
            var updatedOrder = new Order
            {
                SourceId = 99,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                RequestDate = DateTime.UtcNow,
                Reference = "UpdatedRef",
                ShipTo = "ClientZ",
                BillTo = "ClientZ",
                ShipmentId = 99,
                TotalAmount = 500.0,
                TotalTax = 50.0,
                TotalDiscount = 10.0,
                TotalSurcharge = 5.0,
            };

            var result = await _orderService.UpdateOrderAsync(1, updatedOrder);

            Assert.True(result);

            var updatedOrderInDb = await _orderService.GetOrderByIdAsync(1);

            Assert.NotNull(updatedOrderInDb);
            Assert.Equal("ClientZ", updatedOrderInDb.ShipTo);
            Assert.Equal(500.0, updatedOrderInDb.TotalAmount);
        }

        [Fact]
        public async Task TestDeleteOrderAsync()
        {
            var result = await _orderService.DeleteOrderAsync(1);

            Assert.True(result);

            var deletedOrder = await _orderService.GetOrderByIdAsync(1);
            Assert.Null(deletedOrder);
        }

        [Fact]
        public async Task TestGetOrdersForClientAsync()
        {
            var orders = await _orderService.GetOrdersForClientAsync("ClientA");

            Assert.NotNull(orders);
            Assert.Single(orders);
            Assert.Equal(1, orders.First().Id);
        }

        [Fact]
        public async Task TestGetOrdersForShipmentAsync()
        {
            var orders = await _orderService.GetOrdersForShipmentAsync(1);

            Assert.NotNull(orders);
            Assert.Equal(2, orders.Count); //Shipmentid 1 has 2 orders
        }

        [Fact]
        public async Task TestUpdateNonExistingOrderAsync()
        {
            var updatedOrder = new Order
            {
                SourceId = 99,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                RequestDate = DateTime.UtcNow,
                Reference = "UpdatedRef",
                ShipTo = "ClientZ",
                BillTo = "ClientZ",
                ShipmentId = 99,
                TotalAmount = 500.0,
                TotalTax = 50.0,
                TotalDiscount = 10.0,
                TotalSurcharge = 5.0,
            };

            //update an order that does not exist
            var result = await _orderService.UpdateOrderAsync(999, updatedOrder);


            Assert.False(result);
        }

        [Fact]
        public async Task TestDeleteNonExistingOrderAsync()
        {
            //delete  order that does not exist
            var result = await _orderService.DeleteOrderAsync(999);


            Assert.False(result);
        }

        [Fact]
        public async Task TestGetOrdersForClientThatDoesNotExistAsync()
        {
            //get orders for a client that does not exist 
            var orders = await _orderService.GetOrdersForClientAsync("NonExistingClient");


            Assert.NotNull(orders);
            Assert.Empty(orders);
        }

        [Fact]
        public async Task TestGetOrdersForShipmentThatDoesNotExistAsync()
        {
            //get orders for a shipment that does not exist
            var orders = await _orderService.GetOrdersForShipmentAsync(999);

            Assert.NotNull(orders);
            Assert.Empty(orders);
        }

    }
}
