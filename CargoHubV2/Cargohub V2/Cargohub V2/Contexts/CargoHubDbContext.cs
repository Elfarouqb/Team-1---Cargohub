using Cargohub_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargohub_V2.Contexts
{
    public class CargoHubDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public CargoHubDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.Property(e => e.Id)
                    .UseIdentityAlwaysColumn(); // Use default identity settings
            });

            // Configure one-to-many relationship between Order and StockOfItems
            // Configure relationships between entities here
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)  // An Order has many OrderItems
                .WithOne()                   // Each OrderItem has one Order
                .HasForeignKey(oi => oi.OrderId) // Foreign key in OrderItem
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for related OrderItems when Order is deleted

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
