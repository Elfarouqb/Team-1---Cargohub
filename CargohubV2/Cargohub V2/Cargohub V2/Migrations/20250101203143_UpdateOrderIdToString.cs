using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargohubV2.Migrations
{
    public partial class UpdateOrderIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Change the OrderId column from integer to text (string)
            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "Shipments",
                type: "text",  // Or "varchar" if preferred
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the OrderId column from text back to integer
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Shipments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

