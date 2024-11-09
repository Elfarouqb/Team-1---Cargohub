namespace Cargohub_V2.DataConverters
{
    using System.Collections.Generic;
    using System.IO;
    using Cargohub_V2.Contexts;
    using Cargohub_V2.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class DataLoader
    {
        public static List<T> LoadDataFromFile<T>(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
        public static void ImportData(CargoHubDbContext context)
        {
            // Import Clients
            var clients = LoadDataFromFile<Client>("data/clients.json");
            foreach (var client in clients)
            {
                client.Id = 0; // Resetting the Id to 0
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();

            // Import Inventories
            var inventories = LoadDataFromFile<Inventory>("data/inventories.json");
            foreach (var inventory in inventories)
            {
                inventory.Id = 0; // Resetting the Id to 0
            }
            context.Inventories.AddRange(inventories);
            context.SaveChanges();

            // Import Item Groups before Items
            var itemGroups = LoadDataFromFile<Item_Group>("data/item_groups.json");
            foreach (var itemGroup in itemGroups)
            {
                itemGroup.Id = 0; // Resetting the Id to 0
            }
            context.Items_Groups.AddRange(itemGroups);
            context.SaveChanges(); // Ensure Item Groups are saved first

            // Import Item Lines before Items
            var itemLines = LoadDataFromFile<Item_Line>("data/item_lines.json");
            foreach (var itemLine in itemLines)
            {
                itemLine.Id = 0; // Resetting the Id to 0
            }
            context.Items_Lines.AddRange(itemLines);
            context.SaveChanges(); // Ensure Item Lines are saved first

            // Import Item Types before Items
            var itemTypes = LoadDataFromFile<Item_Type>("data/item_types.json");
            foreach (var itemType in itemTypes)
            {
                itemType.Id = 0; // Resetting the Id to 0
            }
            context.Items_Types.AddRange(itemTypes);
            context.SaveChanges(); // Ensure Item Types are saved first

            // Now Import Items
            var items = LoadDataFromFile<Item>("data/items.json");
            foreach (var item in items)
            {
                item.Id = 0; // Resetting the Id to 0
                             // Optionally, ensure the ItemLineId and ItemTypeId are valid before adding
            }
            context.Items.AddRange(items);
            context.SaveChanges();

            // Import Warehouses
            var warehouses = LoadDataFromFile<Warehouse>("data/warehouses.json");
            foreach (var warehouse in warehouses)
            {
                warehouse.Id = 0; // Resetting the Id to 0
            }
            context.Warehouses.AddRange(warehouses);
            context.SaveChanges();

            // Import Suppliers
            var suppliers = LoadDataFromFile<Supplier>("data/suppliers.json");
            foreach (var supplier in suppliers)
            {
                supplier.Id = 0; // Resetting the Id to 0
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            // Import Orders
            var orders = LoadDataFromFile<Order>("data/orders.json");
            foreach (var order in orders)
            {
                order.Id = 0; // Resetting the Id to 0
            }
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // Load Shipments
            var shipments = LoadDataFromFile<Shipment>("data/shipments.json");
            foreach (var shipment in shipments)
            {
                shipment.Id = 0; // Resetting the Id to 0
            }
            context.Shipments.AddRange(shipments);
            context.SaveChanges();

            // Load Transfers
            var transfers = LoadDataFromFile<Transfer>("data/transfers.json");
            foreach (var transfer in transfers)
            {
                transfer.Id = 0; // Resetting the Id to 0
            }
            context.Transfers.AddRange(transfers);
            context.SaveChanges();

            // Load Stocks from Shipments
            foreach (var shipment in shipments)
            {
                if (shipment.Stocks != null)
                {
                    foreach (var item in shipment.Stocks)
                    {
                        var stock = new ShipmentStock
                        {
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            ShipmentId = shipment.Id,
                        };
                        context.Stocks.Add(stock);
                    }
                    context.SaveChanges();
                }
            }

            // Load Stocks from Transfers
            foreach (var transfer in transfers)
            {
                if (transfer.Stocks != null)
                {
                    foreach (var item in transfer.Stocks)
                    {
                        var stock = new TransferStock
                        {
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            TransferId = transfer.Id,
                        };
                        context.Stocks.Add(stock);
                    }
                    context.SaveChanges();
                }
            }

            // Load Stocks from Orders
            foreach (var order in orders)
            {
                if (order.Stocks != null)
                {
                    foreach (var item in order.Stocks)
                    {
                        var stock = new OrderStock
                        {
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            OrderId = order.Id,
                        };
                        context.Stocks.Add(stock);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
