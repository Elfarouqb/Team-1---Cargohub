using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CargohubV2.Migrations
{
    /// <inheritdoc />
    public partial class SetShipmentIdStartValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Modify the sequence to start at 2000
            migrationBuilder.Sql("ALTER SEQUENCE \"Shipments_Id_seq\" RESTART WITH 2000;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the sequence to its default behavior
            migrationBuilder.Sql("ALTER SEQUENCE \"Shipments_Id_seq\" RESTART WITH 1;");
        }
    }
}
