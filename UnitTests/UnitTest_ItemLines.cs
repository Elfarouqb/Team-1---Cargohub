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
    public class UnitTest_ItemLines
    {
        private CargoHubDbContext _dbContext;
        private ItemLineService _itemLineService;
        public UnitTest_Order()
        {
            // In-memory database for testing (no need for PostgreSQL credentials)
            var options = new DbContextOptionsBuilder<CargoHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestItemLineTypeDatabase")
                .Options;

            // Initialize the DbContext with the in-memory database options
            _dbContext = new CargoHubDbContext(options);

            // Seed the database
            SeedDatabase(_dbContext);

            // Initialize the OrderService
            _itemLineService = new ItemLineService(_dbContext);
        }

        private void SeedDatabase(CargoHubDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //fill with seeddata
        }

    }

}