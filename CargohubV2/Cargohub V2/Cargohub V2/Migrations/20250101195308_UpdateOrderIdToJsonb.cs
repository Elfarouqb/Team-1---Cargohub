using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargohubV2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderIdToJsonb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "OrderId",
                table: "Shipments",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Shipments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "jsonb");
        }
    }
}
