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
    public class UnitTest_Shipment
    {
        private CargoHubDbContext _dbContext;
        private ShipmentService _shipmentService;

        public UnitTest_Shipment()
        {
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestShipmentDatabase")
                .Options;

            _dbContext = new CargoHubDbContext(options);
            SeedDatabase(_dbContext);
            _shipmentService = new ShipmentService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Seed Orders
            context.Orders.Add(new Order { Id = 1 });
            context.Orders.Add(new Order { Id = 2 });
            context.Orders.Add(new Order { Id = 3 });
            context.Orders.Add(new Order { Id = 4 });

            //Seed OrderItems
            context.OrderItems.Add(new OrderItem { OrderId = 1, ItemId = "P007435", Amount = 2 });
            context.OrderItems.Add(new OrderItem { OrderId = 1, ItemId = "P007436", Amount = 3 });
            context.OrderItems.Add(new OrderItem { OrderId = 2, ItemId = "P007437", Amount = 4 });
            context.OrderItems.Add(new OrderItem { OrderId = 2, ItemId = "P007438", Amount = 5 });
            context.OrderItems.Add(new OrderItem { OrderId = 3, ItemId = "P007439", Amount = 6 });
            context.OrderItems.Add(new OrderItem { OrderId = 3, ItemId = "P007440", Amount = 7 });
            context.OrderItems.Add(new OrderItem { OrderId = 4, ItemId = "P007441", Amount = 8 });
            context.OrderItems.Add(new OrderItem { OrderId = 4, ItemId = "P007442", Amount = 9 });

            //Seed Shipments
            context.Shipments.Add(new Shipment
            {
                Id = 1,
                SourceId = 1,
                OrderId = "4",  //Order with Id 4
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

            context.SaveChanges();
        }
        [Fact]
        public async Task TestAddShipment()
        {
            //new shipment 
            var newShipment = new Shipment
            {
                Id = 3,
                SourceId = 3,
                OrderId = "1,2",
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

            var result = await _shipmentService.AddShipmentAsync(newShipment);

            Assert.NotNull(result);
            Assert.Equal(newShipment.OrderId, result.OrderId);

            //Verify shipmentitems for the orderids
            var shipmentItems = await _dbContext.ShipmentItems
                .Where(si => si.ShipmentId == result.Id)
                .ToListAsync();

            Assert.NotNull(shipmentItems);
            Assert.Equal(4, shipmentItems.Count); //count how many items are in the shipment

            //Verify ShipmentItems
            Assert.Contains(shipmentItems, item => item.ItemId == "P007435" && item.Amount == 2); //OrderId 1
            Assert.Contains(shipmentItems, item => item.ItemId == "P007436" && item.Amount == 3); //OrderId 1
            Assert.Contains(shipmentItems, item => item.ItemId == "P007437" && item.Amount == 4); //OrderId 2
            Assert.Contains(shipmentItems, item => item.ItemId == "P007438" && item.Amount == 5); //OrderId 2
        }

        [Fact]
        public async Task TestUpdateShipment()
        {
            // Arrange: Create the updated shipment
            var updatedShipment = new Shipment
            {
                Id = 1,
                SourceId = 1,
                OrderId = "1,2,3,4", // Orders with Ids 1, 2, 3, and 4
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

            // Act: Update the shipment
            var result = await _shipmentService.UpdateShipmentAsync(1, updatedShipment);

            // Assert: Verify the shipment update was successful
            Assert.True(result);

            // Fetch the updated shipment from the database
            var updatedShipmentInDb = await _dbContext.Shipments
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == 1);

            Assert.NotNull(updatedShipmentInDb);
            Assert.Equal(updatedShipment.OrderId, updatedShipmentInDb.OrderId);
            Assert.Equal("Delivered", updatedShipmentInDb.ShipmentStatus);

            // Verify the updated ShipmentItems match the OrderItems for the given OrderId
            var orderIds = updatedShipment.OrderId.Split(',').ToList(); // Materialize OrderIds in memory
            var expectedShipmentItems = await _dbContext.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId.ToString()))
                .ToListAsync();

            Assert.NotNull(updatedShipmentInDb.Items);
            Assert.Equal(expectedShipmentItems.Count, updatedShipmentInDb.Items.Count);

            foreach (var expectedItem in expectedShipmentItems)
            {
                Assert.Contains(updatedShipmentInDb.Items, item =>
                    item.ItemId == expectedItem.ItemId && item.Amount == expectedItem.Amount);
            }
        }



        [Fact]
        public async Task TestRemoveShipmentWithNoRelatedOrders()
        {
            var newShipment = new Shipment
            {
                Id = 4,
                SourceId = 4,
                OrderId = "5", //OrderId that does not exist
                OrderDate = DateTime.UtcNow.ToString("o"),
                RequestDate = DateTime.UtcNow.ToString("o"),
                ShipmentDate = DateTime.UtcNow.ToString("o"),
                ShipmentType = "Standard",
                ShipmentStatus = "Pending",
                Notes = "Shipment with no related orders",
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

            var result = await _shipmentService.AddShipmentAsync(newShipment);
            Assert.NotNull(result);

            var removeResult = await _shipmentService.RemoveShipmentAsync(newShipment.Id);
            Assert.True(removeResult);
        }

        [Fact]
        public async Task TestUpdateNonExistentShipment()
        {
            var updatedShipment = new Shipment
            {
                Id = 999, // id does not exist
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

            await Assert.ThrowsAsync<Exception>(async () => await _shipmentService.UpdateShipmentAsync(999, updatedShipment));
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        public async Task TestRemoveShipment(int id, bool expected)
        {
            var result = await _shipmentService.RemoveShipmentAsync(id);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task TestGetItemsInShipmentAsync()
        {
            //Post a shipment  OrderIds
            var newShipment = new Shipment
            {
                Id = 5,
                SourceId = 1,
                OrderId = "1,2", // Orders Ids 1 , 2
                OrderDate = DateTime.UtcNow.ToString("o"),
                RequestDate = DateTime.UtcNow.ToString("o"),
                ShipmentDate = DateTime.UtcNow.ToString("o"),
                ShipmentType = "Standard",
                ShipmentStatus = "Pending",
                Notes = "New test shipment",
                CarrierCode = "DHL",
                CarrierDescription = "DHL Standard",
                ServiceCode = "STF",
                PaymentType = "Prepaid",
                TransferMode = "Truck",
                TotalPackageCount = 2,
                TotalPackageWeight = 20.0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //Add the shipment
            var postedShipment = await _shipmentService.AddShipmentAsync(newShipment);

            // Verify shipment
            Assert.NotNull(postedShipment);

            //items in shipment
            var shipmentItems = await _shipmentService.GetItemsInShipmentAsync(postedShipment.Id);

            //items match OrderItems OrderIds 1 , 2
            Assert.NotNull(shipmentItems);


            var orderIds = newShipment.OrderId.Split(',').Select(int.Parse).ToList();

            //expected OrderItems
            var expectedOrderItems = await _dbContext.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId))
                .ToListAsync();

            Assert.Equal(expectedOrderItems.Count, shipmentItems.Count);

            foreach (var expectedItem in expectedOrderItems)
            {
                Assert.Contains(shipmentItems, item =>
                    item.ItemId == expectedItem.ItemId && item.Amount == expectedItem.Amount);
            }
        }



        [Fact]
        public async Task TestGetItemsInNonExistentShipmentAsync()
        {
            //Test shipment non-existent ID
            var shipmentItems = await _shipmentService.GetItemsInShipmentAsync(999);

            Assert.NotNull(shipmentItems);
            Assert.Empty(shipmentItems);
        }

        [Fact]
        public async Task TestUpdateItemsInShipmentAsync()
        {
            //items to update for shipment 1
            var updatedItems = new List<ShipmentItem>
            {
                new ShipmentItem { ItemId = "P007439", Amount = 4 },
                new ShipmentItem { ItemId = "P007440", Amount = 5 }
            };

            var result = await _shipmentService.UpdateItemsInShipmentAsync(1, updatedItems);

            Assert.True(result);

            var shipment = await _dbContext.Shipments
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == 1);

            Assert.NotNull(shipment);
            Assert.Equal(2, shipment.Items.Count);
            Assert.Contains(shipment.Items, item => item.ItemId == "P007439" && item.Amount == 4);
            Assert.Contains(shipment.Items, item => item.ItemId == "P007440" && item.Amount == 5);
        }

        [Fact]
        public async Task TestUpdateItemsInNonExistentShipmentAsync()
        {
            //items to update for a non-existent shipment
            var updatedItems = new List<ShipmentItem>
            {
                new ShipmentItem { ItemId = "P007441", Amount = 7 },
                new ShipmentItem { ItemId = "P007442", Amount = 8 }
            };

            //updating items for shipment ID = 999
            var result = await _shipmentService.UpdateItemsInShipmentAsync(999, updatedItems);


            Assert.False(result);
        }
    }
}



