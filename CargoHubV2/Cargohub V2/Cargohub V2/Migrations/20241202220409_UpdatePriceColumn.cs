using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cargohub_V2.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Suppliers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Suppliers");
        }
    }
}
