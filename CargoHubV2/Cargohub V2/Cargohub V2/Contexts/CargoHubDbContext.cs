﻿using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Contexts
{
    public class CargoHubDbContext : DbContext
    {
        public CargoHubDbContext(DbContextOptions<CargoHubDbContext> options)
            : base(options) // This is required for DbContext to work correctly
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipment)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ShipmentId)
                .OnDelete(DeleteBehavior.SetNull); // Set ShipmentId to null when Shipment is deleted

            modelBuilder.Entity<Warehouse>()
                .OwnsOne(w => w.Contact);
            modelBuilder.Entity<Stock>()
                .ToTable("Stocks")
                .HasDiscriminator<string>("StockType")
                .HasValue<OrderStock>("Order")
                .HasValue<ShipmentStock>("Shipment")
                .HasValue<TransferStock>("Transfer");
        }


        public DbSet<Client> Clients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Item_Group> Items_Groups { get; set; }
        public DbSet<Item_Line> Items_Lines { get; set; }
        public DbSet<Item_Type> Items_Types { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }

    }
}
