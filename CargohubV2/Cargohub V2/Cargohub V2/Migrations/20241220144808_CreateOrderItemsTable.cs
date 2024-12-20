using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CargohubV2.Migrations
{
    public partial class CreateOrderItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn), // Auto-increment primary key
                    ItemId = table.Column<int>(type: "int", nullable: false), // Use ItemId as integer
                    UId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false), // Keep UId for reference
                    Amount = table.Column<int>(type: "int", nullable: false), // Amount
                    OrderId = table.Column<int>(type: "int", nullable: false) // Foreign Key to Orders
                },
                constraints: table =>
                {
                    // Primary Key for the OrderItems table
                    table.PrimaryKey("PK_OrderItems", x => x.Id);

                    // Foreign Key to Orders
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    // Foreign Key to Items (referencing the 'Id' column in Items table)
                    table.ForeignKey(
                        name: "FK_OrderItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",  // Using the Id column from Items
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes for faster querying
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemId",
                table: "OrderItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the OrderItems table if the migration is rolled back
            migrationBuilder.DropTable(
                name: "OrderItems");
        }
    }
}





