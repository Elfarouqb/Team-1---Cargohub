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
            // Add the explicit cast to jsonb using the USING clause
            migrationBuilder.Sql(
                "ALTER TABLE \"Shipments\" ALTER COLUMN \"OrderId\" TYPE jsonb USING \"OrderId\"::jsonb;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // If you want to rollback, you may want to convert jsonb back to its original type, e.g., integer
            migrationBuilder.Sql(
                "ALTER TABLE \"Shipments\" ALTER COLUMN \"OrderId\" TYPE integer USING \"OrderId\"::integer;"
            );
        }
    }
}

