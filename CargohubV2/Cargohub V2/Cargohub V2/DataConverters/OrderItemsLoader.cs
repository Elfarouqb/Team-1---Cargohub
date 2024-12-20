using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cargohub_V2.Contexts;
using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.DataConverters
{
    public class OrderItemsProcessor
    {
        private readonly CargoHubDbContext _context;

        public OrderItemsProcessor(CargoHubDbContext context)
        {
            _context = context;
        }

        public async Task ProcessOrderItemsAsync(string orderJsonFilePath)
        {
            // Load the orders data from the JSON file
            var orders = LoadDataFromFile<Order>("data/orders.json");

            // List to hold the OrderItems that will be inserted into the database
            var orderItemsList = new List<OrderItem>();

            foreach (var order in orders)
            {
                // Loop through each item in the order and populate OrderItems table
                foreach (var orderItem in order.OrderItems)
                {
                    // Initialize the OrderItem entity with OrderId and Amount
                    var orderItemEntity = new OrderItem
                    {
                        UId = orderItem.UId,  // Store the item_id as UId for now
                        Amount = orderItem.Amount,
                        OrderId = order.Id  // Set the OrderId from the current order
                    };

                    // Look up the corresponding item in the Items table based on UId
                    var item = await _context.Items.FirstOrDefaultAsync(i => i.UId == orderItem.UId);
                    if (item != null) // If a matching item is found
                    {
                        // Set the ItemId from the corresponding Item found in the database
                        orderItemEntity.ItemId = item.Id;
                    }
                    else
                    {
                        // If no matching item is found, log it or skip
                        Console.WriteLine($"Item with UId {orderItem.UId} not found for OrderId {order.Id}");
                        continue;  // Skip this orderItem if no matching item is found
                    }

                    // Add the populated orderItemEntity to the list
                    orderItemsList.Add(orderItemEntity);
                }
            }

            // Add all the order items to the database
            if (orderItemsList.Any())
            {
                await _context.OrderItems.AddRangeAsync(orderItemsList);
                await _context.SaveChangesAsync(); // Save the order items into the OrderItems table
            }
        }

        private List<T> LoadDataFromFile<T>(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);  // Read the JSON file
            return JsonConvert.DeserializeObject<List<T>>(jsonData);  // Deserialize the JSON data into a list of objects
        }
    }
}

