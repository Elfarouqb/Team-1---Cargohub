using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargohubV2.Migrations
{
    public partial class AddShipmentItemsToShipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the ShipmentItems table
            migrationBuilder.CreateTable(
                name: "ShipmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false), // Changed to varchar
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentItems_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Optionally create an index on ShipmentId for faster queries
            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItems_ShipmentId",
                table: "ShipmentItems",
                column: "ShipmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ShipmentItems");
        }
    }
}

