using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

//use dotnet test for running tests
namespace UnitTests
{
    public class UnitTest_Shipment
    {
        private CargoHubDbContext _dbContext;
        private ShipmentService _shipmentService;

        public UnitTest_Shipment()
        {
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestItemDatabase")
                .Options;

            _dbContext = new CargoHubDbContext(options);
            SeedDatabase(_dbContext);
            _shipmentService = new ShipmentService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Items.AddRange(
            //fill data 
            new Item { },

        );
        }
    }
}