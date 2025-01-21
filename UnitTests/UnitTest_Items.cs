using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Cargohub_V2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace UnitTests
{
    public class UnitTest_Item
    {
        private CargoHubDbContext _dbContext;
        private ItemService _itemService;
        public UnitTest_Order()
        {
            // In-memory database for testing (no need for PostgreSQL credentials)
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestItemDatabase")
                .Options;

            // Initialize the DbContext with the in-memory database options
            _dbContext = new CargoHubDbContext(options);

            // Seed the database
            SeedDatabase(_dbContext);

            // Initialize the OrderService
            _itemService = new ItemService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //fill with seeddata
        }

    }

}